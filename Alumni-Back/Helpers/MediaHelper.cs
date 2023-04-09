using Alumni_Back.Models;
using Alumni_Back.Repository;

namespace Alumni_Back.Helpers
{
    public class MediaHelper : IMediaRepository
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;
        private readonly string media_directory;
        public MediaHelper(IWebHostEnvironment _environment,IConfiguration configuration)
        {
            this.environment = _environment;
            this.configuration = configuration;
            this.media_directory = this.environment.WebRootPath + "\\Medias";
        }

        public string ConfigureUrl(string path)
        {
            string server = this.configuration.GetValue<string>("Server");
            return server + path;
        }

        public async Task<byte[]> Load(string path)
        {
            path = this.media_directory + "\\"+path;
            if (System.IO.File.Exists(path))
            {
                byte[] b = await System.IO.File.ReadAllBytesAsync(path);
                return b;
            }
            return null;
        }

        public async Task Upload(FileUpload file, string path)
        {
            //path=this.media_directory + path;
            path = this.media_directory + "\\" + path;
            Console.WriteLine(path);
            if (!Directory.Exists(media_directory))
            {
                Directory.CreateDirectory(media_directory);
            }
            using (FileStream stream = System.IO.File.Create(path))
            {
                file.File.CopyTo(stream);
                stream.Flush();
            }
        }
    }
}
