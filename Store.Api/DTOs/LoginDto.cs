using System.ComponentModel.DataAnnotations;

namespace Store.Api.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string  Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
