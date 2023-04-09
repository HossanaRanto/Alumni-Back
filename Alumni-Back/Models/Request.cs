using System.Text.Json.Serialization;

namespace Alumni_Back.Models
{
    public class Request
    {
        public int Id { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public University University { get; set; }
        public DateTime Created_at { get; set; }=DateTime.Now;
        //public bool IsAccepted { get; set; } = false;
    }
}
