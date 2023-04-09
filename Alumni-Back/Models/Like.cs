namespace Alumni_Back.Models
{
    public class Like
    {
        public int Id { get; set; }
        public User Liker { get; set; }
        public DateTime Like_at { get; set; }= DateTime.Now;
        public Post Post { get; set; }
    }
}
