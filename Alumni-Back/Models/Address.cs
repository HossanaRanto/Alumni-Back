using Alumni_Back.DTO;

namespace Alumni_Back.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string? Coordinates { get; set; }
    }
}
