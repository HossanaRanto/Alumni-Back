namespace Alumni_Back.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime DateSend { get; set; }= DateTime.Now;
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string Content { get; set; }
    }
}
