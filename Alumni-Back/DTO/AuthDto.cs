using System.ComponentModel.DataAnnotations;

namespace Alumni_Back.DTO
{
    public class AuthDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
