using QualifiedAuthentication.Models.Register;
using QualifiedAuthentication.Models.Token;
using System.Security.Claims;

namespace QualifiedAuthentication.Interfaces
{
    public interface ITokenService
    {
        string? GenerateRefreshToken();
        string? GenerateAccessToken(string? userName);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Token? GenerateNewRefreshToken(Token token);
        Token RegisterRefreshToken(UserResponse user);
    }
}
