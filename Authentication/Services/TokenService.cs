using Microsoft.IdentityModel.Tokens;
using QualifiedAuthentication.Interfaces;
using QualifiedAuthentication.Models.Data;
using QualifiedAuthentication.Models.Register;
using QualifiedAuthentication.Models.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace QualifiedAuthentication.Services
{
    public class TokenService : ITokenService
    {
        IConfiguration _configuration;
        IDatabaseService _databaseService;
        public TokenService(IConfiguration configuration,IDatabaseService databaseService)
        {
            _configuration = configuration; 
            _databaseService = databaseService;
        }
        public string? GenerateAccessToken(string? username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Aud, _configuration["JWT:Audience"]),
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["JWT:Issuer"])
                },
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string? GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public Token RegisterRefreshToken(UserResponse user)
        {
            var accessToken = GenerateAccessToken(user.Username);
            var refreshToken = GenerateRefreshToken();

            _databaseService.InsertRefreshToken(new UserRefreshToken()
            {
                Id = user.Id,
                Username = user.Username,
                RefreshToken = refreshToken,
                IsActive = true
            });

            return new Token { AccessToken = accessToken , RefreshToken = refreshToken};
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        public Token? GenerateNewRefreshToken(Token token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var name = principal.Identity?.Name;
            var accessToken = GenerateAccessToken(name);
            
            var userRefreshToken = _databaseService.GetUserRefreshToken(token.RefreshToken);

            if (userRefreshToken == null || userRefreshToken.RefreshToken != token.RefreshToken)
                return null;

            var refreshToken = GenerateRefreshToken();
            userRefreshToken.RefreshToken = refreshToken;

            _databaseService.InsertRefreshToken(userRefreshToken);

            return new Token()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
