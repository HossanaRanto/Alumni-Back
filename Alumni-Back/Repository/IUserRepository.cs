using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Serializers;
using Alumni_Back.Validators;
using OneOf;

namespace Alumni_Back.Repository
{
    public interface IUserRepository
    {
        User ConnectedUser { get; set; }
        Task<UserSerializer> GetSerializer();
        Task<UserSerializer> GetSerializer(int user);
        Task<UserSerializer> GetSerializer(User user);
        Task<object> CheckUsername(string username,bool isnew);
        Task<object> CheckEmail(string email,bool isnew);
        string HashPassword(string password);
        OneOf<User, ValidationFailed> Create(UserDto user);
        OneOf<User, ValidationFailed> Update(UserDto user,bool updatepassword);
        Task<OneOf<string, Token>> Login(AuthDto auth);
        OneOf<string,User> Get(string username);
        OneOf<string, User> Get(int id);
        string GetRole(User user);
        Task<OneOf<string,Token>> RefreshToken(string token);
        Task<byte[]> Load(string param);
        Task Upload(FileUpload file,string isProfilpicture);
        Task<string> GetPicture(User user, string mediatype);
        Task<string> CurrentProfilPicture(User user);
        Task<string> CurrentCoverPicture(User user);
        Task<Media> UpdatePicture(User user, string isProfilpicture, string filename = null);
        Task<List<string>> GetPictures(User user, int? offset,int? limit);
        Task<List<string>> GetPictures(int user, int? offset, int? limit);
        Task<List<string>> GetPictures(int? offset, int? limit);
    }
}
