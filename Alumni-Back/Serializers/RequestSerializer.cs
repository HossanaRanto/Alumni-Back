using Alumni_Back.Models;

namespace Alumni_Back.Serializers
{
    public class RequestSerializer
    {
        public UserSerializer User { get; set; }
        public Request Request { get; set; }
    }
}
