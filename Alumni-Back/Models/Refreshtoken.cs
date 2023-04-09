namespace Alumni_Back.Models
{
    public class Refreshtoken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime created_at { get; } = DateTime.Now;
    }
}
