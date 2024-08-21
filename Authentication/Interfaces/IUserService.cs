using QualifiedAuthentication.Models.Register;
using QualifiedAuthentication.Models.User;

namespace QualifiedAuthentication.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse?> LoginAsync(UserLogin users);
        Task<UserResponse?> RegisterAsync(UserRequest user);
    }
}
