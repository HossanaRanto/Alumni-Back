using System.ComponentModel.DataAnnotations;

namespace Alumni_Back.DTO
{
    public class UniversityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressDto Address { get; set; }
        [Phone]
        public string Contact { get; set; }
        [EmailAddress] public string Email { get; set; }
    }
}
