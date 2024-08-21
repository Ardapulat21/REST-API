using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using QualifiedAuthentication.Interfaces;
using QualifiedAuthentication.Models.Register;
using QualifiedAuthentication.Models.Token;
using QualifiedAuthentication.Models.User;
using System.Runtime.CompilerServices;
namespace QualifiedAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IDatabaseService _databaseService;
        public UserController(IUserService userService, ITokenService tokenService, IDatabaseService databaseService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _databaseService = databaseService;
        }

        [Authorize]
        [HttpGet]
        [Route("GellAllAccounts")]
        public List<UserResponse> Get()
        {
            return _databaseService.GetAllCredentials();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLogin user)
        {
            var validUser = await _userService.LoginAsync(user);

            if (validUser == null)
                return Unauthorized("Invalid username or password...");

            Token token = _tokenService.RegisterRefreshToken(validUser);

            return Ok($"Access Token: {token.AccessToken}\nRefresh Token: {token.RefreshToken}");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(Models.Register.UserRequest user)
        {
            var result = await _userService.RegisterAsync(user);

            return result == null
              ? Unauthorized($"User could not be registered.")
              : Content("User created successfully.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh-Token")]
        public IActionResult Refresh(Token token)
        {
            var newToken = _tokenService.GenerateNewRefreshToken(token);
            return Ok($"New Acces token is: {newToken.AccessToken}\nNew Refresh token is: {newToken.RefreshToken}");
        }
    }
}
