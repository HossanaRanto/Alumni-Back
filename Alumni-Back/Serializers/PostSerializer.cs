using Alumni_Back.Models;

namespace Alumni_Back.Serializers
{
    public class PostSerializer
    {
        public Post Post { get; set; }
        public bool IsLiked { get; set; }
    }
}
