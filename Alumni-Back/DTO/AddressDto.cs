using Alumni_Back.Models;

namespace Alumni_Back.DTO
{
    public class AddressDto
    {
        public string country { get; set; }
        public string city { get; set; }
        public int postalcode { get; set; }
        public string? coordinates { get; set; }
    }
}
