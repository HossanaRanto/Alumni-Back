using Alumni_Back.Models;

namespace Alumni_Back.Serializers
{
    public class CommentSerializer
    {
        public User Commentator { get; set; }
        public string Content { get; set; }
    }
}
