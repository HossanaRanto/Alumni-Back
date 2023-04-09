namespace Alumni_Back.Models
{
    public class Commentary
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public User Commentator { get; set; }
        public DateTime Comment_at { get; set; }=DateTime.Now;
        public Post Post { get; set; }
    }
}
