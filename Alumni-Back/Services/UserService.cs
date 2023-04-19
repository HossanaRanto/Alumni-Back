using Alumni_Back.DTO;
using Alumni_Back.Helpers;
using Alumni_Back.Models;
using Alumni_Back.Repository;
using Alumni_Back.Serializers;
using Alumni_Back.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OneOf;
using OneOf.Types;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Alumni_Back.Services
{
    public class UserService : IUserRepository
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        private readonly IJwtRepository jwt;
        private readonly IMediaRepository media;
        //private readonly IHttpContextAccessor http;

        public User ConnectedUser { get; set; }
        public UserService(
            DataContext context,
            IConfiguration configuration,
            IJwtRepository jwt,
            IMediaRepository media
            //IHttpContextAccessor http
            )
        {
            this.context = context;
            this.configuration=configuration;
            this.jwt = jwt;
            this.media = media;
            //this.http = http;
        }
        public OneOf<User,ValidationFailed> Create(UserDto user)
        {
            var validator = new UserValidator();
            var validate = validator.Validate(user);
            if (!validate.IsValid)
            {
                return new ValidationFailed(validate.Errors);
            }
            var address = new Address
            {
                Country = user.Address.country,
                City = user.Address.city,
                PostalCode = user.Address.postalcode,
                Coordinates = user.Address.coordinates
            };

            this.context.Addresses.Add(address);
            this.context.SaveChanges();

            var u = new User
            {
                Username = user.Username,
                Password = HashPassword(user.Password),
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Gender = user.Gender,
                Birthdate = user.Birthdate,
                Contact = user.Contact,
                Address = address,
                Email = user.Email,
            };
            context.Users.Add(u);
            context.SaveChanges();
            return u;
        }
        public OneOf<User,ValidationFailed> Update(UserDto user,bool updatepassword)
        {
            var validator = new UserValidator();
            var validate = validator.Validate(user);
            if (!validate.IsValid)
            {
                return new ValidationFailed(validate.Errors);
            }
            var u = this.ConnectedUser;
            u.Username = user.Username;
            if (updatepassword)
            {
                u.Password = HashPassword(user.Password);
            }
            u.Firstname = user.Firstname;
            u.Lastname = user.Lastname;
            u.Gender = user.Gender;
            u.Birthdate = user.Birthdate;
            u.Contact = user.Contact;
            u.Address = new Address
            {
                Country = user.Address.country,
                City = user.Address.city,
                PostalCode = user.Address.postalcode,
                Coordinates = user.Address.coordinates
            };
            u.Email = user.Email;
            context.SaveChanges();
            return u;
        }
        public OneOf<string, User> Get(string username)
        {
            var user = context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return "User not found";
            }
            return user;
        }
        public OneOf<string, User> Get(int id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return "User not found";
            }
            return user;
        }
        public string GetRole(User user)
        {
            var checkifadmin = context.Universities
                .Include(x => x.Adminstrator)
                .FirstOrDefault(u => u.Adminstrator == user);
            if(checkifadmin != null)
            {
                return UserRole.Admin;
            }

            var checkifsimpleuser= context.UniversitiesStudents
                .Include(x => x.User).
                FirstOrDefault(us=>us.User== user);

            if(checkifsimpleuser == null)
            {
                return UserRole.SimpleUser;
            }

            
            if (checkifsimpleuser != null &&
                checkifsimpleuser.Date_quit.HasValue && 
                checkifsimpleuser.Date_quit.Value < DateTime.Now)
            {
                return UserRole.Alumni;
            }

            return UserRole.Student;
        }

        public string HashPassword(string password)
        {
            string key = configuration.GetValue<string>("Key");
            var key_password = Encoding.UTF8.GetBytes(key);
            using (var hmac = new HMACSHA512(key_password))
            {
                var pwd= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(pwd);
                //return Encoding.UTF8.GetString(pwd);
            }
        }

        public async Task<OneOf<string,Token>> Login(AuthDto auth)
        {
            var check=await context.Users.FirstOrDefaultAsync(user=>user.Username==auth.Username);
            if (check == null)
            {
                return "Username not found";
            }
            if (check.Password != HashPassword(auth.Password))
            {
                return "Password incorrect";
            }
            string role = this.GetRole(check);
            var token = jwt.Generate(check,role);
            return token;

        }

        public async Task<OneOf<string, Token>> RefreshToken(string token)
        {
            var checkifused = await this.context.Refreshtokens.FirstOrDefaultAsync(t => t.Token == token && t.IsUsed);
            if (checkifused != null)
            {
                return "Refresh token already used";
            }
            //Generate token
            var token_decode = jwt.Verify(token);
            var user = this.Get(token_decode.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value).AsT1;
            string role = this.GetRole(user);
            var new_token = jwt.Generate(user, role);


            var black_refresh = new Refreshtoken
            {
                Token = token,
                IsUsed = true
            };
            this.context.Refreshtokens.Add(black_refresh);
            await this.context.SaveChangesAsync();

            return new_token;
        }

        public async Task<object> CheckUsername(string username,bool isnew)
        {
            var checkif = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (checkif == null && isnew)
            {
                return new
                {
                    Message = "Username valid",
                    Valid = true
                };
            }
            if(checkif != null && !isnew)
            {
                if (checkif == this.ConnectedUser)
                {
                    return new
                    {
                        Message = "Username valid",
                        Valid = true
                    };
                }
            }
            return new
            {
                Message = "Username invalid",
                Valid = false
            };
        }

        public async Task<object> CheckEmail(string email, bool isnew)
        {
            var checkif = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (checkif == null)
            {
                return new
                {
                    Message = "Email valid",
                    Valid = true
                };
            }
            return new
            {
                Message = "Email invalid",
                Valid = false
            };
        }

        public async Task<byte[]> Load(string param)
        {
            return await media.Load(param);
        }

        public async Task Upload(FileUpload file,string isProfilpicture)
        {
            //var current_media = await this.context.Medias.Include(m => m.User)
            //    .FirstOrDefaultAsync(m => m.User == this.ConnectedUser && m.Ispdp == isProfilpicture);

            //if (current_media != null)
            //{
            //    current_media.IsCurrent = false;
            //}

            //var new_media = new Media
            //{
            //    User = this.ConnectedUser,
            //    IsCurrent = true,
            //    Ispdp = isProfilpicture,
            //    Filename = Guid.NewGuid().ToString()
            //};

            var new_media = await UpdatePicture(this.ConnectedUser, isProfilpicture,file.File.FileName);
            await media.Upload(file, new_media.Filename);

            //await this.context.Medias.AddAsync(new_media);
            //await this.context.SaveChangesAsync();

        }
        public async Task<Media> UpdatePicture(User user,string isProfilpicture,string filename)
        {
            var current_media = await this.context.Medias.Include(m => m.User)
                .FirstOrDefaultAsync(m => m.User == this.ConnectedUser && m.Ispdp == isProfilpicture);

            if (current_media != null)
            {
                current_media.IsCurrent = false;
                this.context.Update<Media>(current_media);
            }
            var new_media = new Media
            {
                User = this.ConnectedUser,
                IsCurrent = true,
                Ispdp = isProfilpicture,
                Filename = Guid.NewGuid() + Path.GetExtension(filename)
        };

            await this.context.Medias.AddAsync(new_media);
            await this.context.SaveChangesAsync();

            return new_media;
        }
        public async Task<string> GetPicture(User user,string media_type)
        {
            var profil_picture = this.context.Medias
                .Include(p => p.User)
                .FirstOrDefault(media => media.IsCurrent && media.User == user && media.Ispdp == media_type);

            string path = profil_picture == null ? "1.png" : profil_picture.Filename;

            return media.ConfigureUrl(path);
        }
        public async Task<string> CurrentProfilPicture(User user)
        {

            return await GetPicture(user, MediaType.PDP);

            //return this.media.ConfigureUrl(user,MediaType.PDP);
        }
        public async Task<string> CurrentCoverPicture(User user)
        {
            return await GetPicture(user, MediaType.PDC);
        }

        public async Task<UserSerializer> GetSerializer(User user)
        {
            return new UserSerializer
            {
                User = user,
                ProfilPicture = await CurrentProfilPicture(user),
                CoverPicture=await CurrentCoverPicture(user),
                Role = this.GetRole(user)
            };
        }
        public async Task<UserSerializer> GetSerializer(int user)
        {
            return await this.GetSerializer(this.Get(user).AsT1);
        }
        public async Task<UserSerializer> GetSerializer()
        {
            return await this.GetSerializer(this.ConnectedUser);
        }

        public async Task<List<string>> GetPictures(User user, int? offset, int? limit)
        {
            var pictures = await this.context.Medias
                .Include(m=>m.User)
                .Where(m => m.User == user)
                .OrderByDescending(m => m.Upload_at)
                .Select(m=>m.Filename)
                .ToListAsync();
            if(offset.HasValue && limit.HasValue)
            {
                pictures = pictures.Skip(offset.Value).Take(limit.Value).ToList();
            }
            return pictures.Select(picture => this.media.ConfigureUrl(picture)).ToList();
        }

        public async Task<List<string>> GetPictures(int user, int? offset, int? limit)
        {
            return await this.GetPictures(this.Get(user).AsT1,offset,limit);
        }

        public async Task<List<string>> GetPictures(int? offset, int? limit)
        {
            return await this.GetPictures(this.ConnectedUser, offset, limit);
        }
    }
}
