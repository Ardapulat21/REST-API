using QualifiedAuthentication.Models.Register;
using QualifiedAuthentication.Models.Token;
using System.Net;

namespace QualifiedAuthentication.Models.Data
{
    public class DataBase
    {
        public List<UserRefreshToken> RefreshTokens { get; set; }
        public List<UserResponse> Users { get; set; }
        public DataBase()
        {
            RefreshTokens = new List<UserRefreshToken>();
            Users = new List<UserResponse>();
        }
    }
}
