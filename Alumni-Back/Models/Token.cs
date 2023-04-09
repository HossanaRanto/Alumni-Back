using Alumni_Back.Serializers;

namespace Alumni_Back.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public User User { get; set; }
        public string Role { get; set; }
    }
}
