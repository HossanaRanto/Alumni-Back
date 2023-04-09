using Alumni_Back.Models;

namespace Alumni_Back.Serializers
{
    public class UniversitySerializer
    {
        public University University { get; set; }
        public bool IsEnrolled { get; set; }
        public bool IsRequestSent { get; set; }
    }
}
