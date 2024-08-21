using QualifiedAuthentication.Models.Register;
using QualifiedAuthentication.Models.Token;
using QualifiedAuthentication.Models.User;

namespace QualifiedAuthentication.Interfaces
{
    public interface IDatabaseService
    {
        public void InsertUser(UserRequest user);
        public UserResponse? GetUser(UserLogin? user);
        public UserResponse? GetUser(string? refreshToken);
        public void InsertRefreshToken(UserRefreshToken userRefreshToken);
        public UserRefreshToken? GetUserRefreshToken(UserRefreshToken refreshToken);
        public UserRefreshToken? GetUserRefreshToken(string? refreshToken);
        public List<UserResponse> GetAllCredentials();
    }
}
