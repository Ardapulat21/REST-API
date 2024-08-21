using System.ComponentModel.DataAnnotations;

namespace QualifiedAuthentication.Models.Register
{
    public class UserRequest
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }
    }
}
