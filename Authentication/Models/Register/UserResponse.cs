using System.ComponentModel.DataAnnotations;

namespace QualifiedAuthentication.Models.Register
{
    public class UserResponse
    {
        public static int Counter = 0;
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
