using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Alumni_Back.Models
{
    public class Media
    {
        public int Id { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        public string Filename { get; set; }
        public string Ispdp { get; set; } = MediaType.PDP;
        public DateTime Upload_at { get; set; } = DateTime.Now;
        public bool IsCurrent { get; set; } = true;
    }
}
