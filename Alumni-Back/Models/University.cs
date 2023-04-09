using System.ComponentModel.DataAnnotations;

namespace Alumni_Back.Models
{
    public class University
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        [Phone]
        public string Contact { get; set; }
        [EmailAddress] public string Email { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public User Adminstrator { get; set; }
        public string ImageCover { get; set; } = "";
    }
}
