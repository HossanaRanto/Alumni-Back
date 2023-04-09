using Alumni_Back.Models;
using System.ComponentModel.DataAnnotations;

namespace Alumni_Back.DTO
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required,MinLength(8)]
        public string ConfirmPassword { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public bool Gender { get; set; } = true;
        public DateTime Birthdate { get; set; }
        [Phone]
        public string Contact { get; set; }

        public new AddressDto Address { get; set; }
    }
}
