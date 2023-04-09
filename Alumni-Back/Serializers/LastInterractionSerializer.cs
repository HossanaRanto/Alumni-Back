using Alumni_Back.Models;

namespace Alumni_Back.Serializers
{
    public class LastInterractionSerializer
    {
        public UserSerializer User { get; set; }
        public string LastMessage { get; set; }
    }
}
