namespace Alumni_Back.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created_at { get; set; }
        public User Receiver { get; set; }
    }
}
