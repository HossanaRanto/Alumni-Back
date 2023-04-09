using Alumni_Back.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Alumni_Back.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool Gender { get; set; } = true;
        public DateTime Birthdate { get; set; }
        [Phone]
        public string Contact { get; set; }
        public Address Address { get; set; }
        public DateTime Created_at { get;}=DateTime.Now;

        
    }
}
