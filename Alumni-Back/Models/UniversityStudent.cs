using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Alumni_Back.Models
{
    public class UniversityStudent
    {
        public int Id { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public University University { get; set; }
        public DateTime Date_join { get; set; } = DateTime.Now;
        public DateTime? Date_quit { get; set; } = null;

        [NotMapped]
        public bool IsAlumni
        {
            get
            {
                if (Date_quit.HasValue)
                {
                    return true;
                }
                return false;
            }
        }
        public bool checkFromState(string state)
        {
            if (state == UserRole.Alumni && IsAlumni)
            {
                return true;
            }
            if(state==UserRole.Student && !IsAlumni)
            {
                return true;
            }
            return false;
        }
    }
}
