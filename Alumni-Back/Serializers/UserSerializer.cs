using Alumni_Back.Models;

namespace Alumni_Back.Serializers
{
    public class UserSerializer
    {
        public User User { get; set; }
        public string Role { get; set; }
        public string ProfilPicture { get; set; }
        public string CoverPicture { get; set; }
    }
}
