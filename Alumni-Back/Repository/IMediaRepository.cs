using Alumni_Back.Models;
using System;
using System.IO;

namespace Alumni_Back.Repository
{
    public interface IMediaRepository
    {
        string ConfigureUrl(string path);
        Task Upload(FileUpload file, string path);
        Task<byte[]> Load(string path);
    }
}
