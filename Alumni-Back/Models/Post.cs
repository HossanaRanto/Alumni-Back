namespace Alumni_Back.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public User Publisher { get; set; }
        public DateTime Post_at { get; set; }= DateTime.Now;
        public string ImagePath { get; set; } = "";
    }
}
