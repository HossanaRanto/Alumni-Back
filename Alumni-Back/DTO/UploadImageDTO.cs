using Alumni_Back.Models;

namespace Alumni_Back.DTO
{
    public class UploadImageDTO
    {
        public FileUpload Image { get; set; }
        public string IsProfilPicture { get; set; }
    }
}
