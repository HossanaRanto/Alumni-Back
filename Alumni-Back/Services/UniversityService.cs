using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Repository;
using Alumni_Back.Serializers;
using Alumni_Back.Validators;
using AlumniBack.Migrations;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace Alumni_Back.Services
{
    public class UniversityService : IUniversityRepository
    {
        private readonly DataContext context;
        private readonly IUserRepository user;
        private readonly IMediaRepository media;

        public UniversityService(DataContext context,IUserRepository user, IMediaRepository media)
        {
            this.context = context;
            this.user = user;
            this.media = media;
        }

        public async Task<OneOf<string,Request>> AsktoJoin(int university_id)
        {
            var university = await context.Universities.FirstOrDefaultAsync(univ => univ.Id == university_id);
            if(university == null)
            {
                return "University not found";
            }
            if (university.Adminstrator == user.ConnectedUser)
            {
                return "You are the admin of that university";
            }
            var checkjoin = await this.CheckRequest(university, user.ConnectedUser);

            var checkifin=await context.UniversitiesStudents.Include(x => x.User).Include(x=>x.University).
                FirstOrDefaultAsync(us=>us.User==user.ConnectedUser && us.University==university);

            if (checkjoin)
            {
                return "You alread ask to join this university";
            }
            if (checkifin != null)
            {
                return "You are already a student in that university";
            }
            var join = new Request
            {
                University = university,
                User = user.ConnectedUser,
            };

            await context.Requests.AddAsync(join);
            await context.SaveChangesAsync();
            return join;
        }

        public bool checkUniversityname(string name)
        {
            var univ = context.Universities.FirstOrDefault(u => u.Name == name);
            return univ==null?false:true;
        }

        public OneOf<University, ValidationFailed> Create(UniversityDto university)
        {
            var validator = new UniversityValidator();
            var validate=validator.Validate(university);

            if (!validate.IsValid)
            {
                return new ValidationFailed(validate.Errors);
            }

            var checkname = context.Universities.FirstOrDefault(u => u.Name == university.Name);
            if (checkname != null)
            {
                return new ValidationFailed(
                    new ValidationFailure("Name", "That name is already given to another university"));
            }
            var address = new Address
            {
                Country = university.Address.country,
                City = university.Address.city,
                PostalCode = university.Address.postalcode,
                Coordinates = university.Address.coordinates
            };
            context.Addresses.Add(address);
            context.SaveChanges();
            var univ = new University
            {
                Name = university.Name,
                Description = university.Description,
                Contact = university.Contact,
                Email = university.Email,
                Address = address,
                Adminstrator = user.ConnectedUser
            };
            context.Universities.Add(univ);
            context.SaveChanges();

            return univ;

        }

        public async Task<List<Request>> GetListRequest()
        {
            var admin=user.ConnectedUser;
            var university = await context.Universities.FirstOrDefaultAsync();
            var list_request = await context.Requests
                .Include(join => join.University)
                .Include(join=>join.User)
                .Where(request => request.University == university)
                .ToListAsync();

            return list_request;
        }

        public Task<List<UniversitySerializer>> GetUniversities()
        {
            return null;
        }

        public async Task<List<UniversitySerializer>> GetUniversities(string? name)
        {
            //var university_student = await this.context.UniversitiesStudents
            //    .Include(us=>us.User)
            //    .Include(us=>us.University)
            //    .Where(us => us.User == this.user.ConnectedUser).ToListAsync();

            var universities = await this.context.Universities.ToListAsync();
            if (name!=null)
            {
               universities = universities.Where(u => u.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            var universities_serialized = await this.GetList(universities);

            return universities_serialized;
        }
        private async Task<List<UniversitySerializer>> GetList(List<University> source)
        {
            List<UniversitySerializer> list = new List<UniversitySerializer>(); ;
            source.ForEach(async univ =>
            {
                univ.ImageCover = this.media.ConfigureUrl(univ.ImageCover);
                var serializer = new UniversitySerializer
                {
                    University = univ,
                    IsEnrolled = await CheckStudent(univ) == null ? false : true,
                    IsRequestSent = await CheckRequest(univ, this.user.ConnectedUser)
                };
                list.Add(serializer);
            });
            return list;
        }
        public OneOf<string, NotFound,ValidationFailed> JoiningValidation(int join_id)
        {
            var join = context.Requests.Include(x=>x.User)
                .Include(x=>x.University).FirstOrDefault(j => j.Id == join_id);
            if (join == null)
            {
                return new NotFound();
            }
            if (join.University.Adminstrator != user.ConnectedUser)
            {
                return new ValidationFailed(
                    new ValidationFailure
                    ("TypeUser", "You are not the administrator of that university"));
            }
            var univ_student = new UniversityStudent
            {
                User = join.User,
                University=join.University
            };
            context.Requests.Remove(join);
            context.UniversitiesStudents.Add(univ_student);
            context.SaveChanges();

            return $"Student {univ_student.User.Id} accepted";
        }
        public async Task<University> Get(int university)
        {
            return await this.context.Universities.FirstOrDefaultAsync(u => u.Id == university);
        }
        public async Task<List<UserSerializer>> ListStudents(int university, string student_state)
        {
            return await this.ListStudents(
                await this.Get(university),
                student_state);
        }
        public async Task<List<UserSerializer>> ListStudents(University university,string student_state)
        {
            var students = await this.context.UniversitiesStudents
                .Include(us => us.User)
                .Include(us => us.University)
                .Where(us => us.University == university)
                .ToListAsync();
            students = students.Where(us => us.checkFromState(student_state)).ToList();
            return await this.GetList(students);
        }
        public async Task<List<UserSerializer>> ListStudents( string student_state)
        {
            var university = await this.context.Universities.Include(u => u.Adminstrator)
                .FirstOrDefaultAsync(u => u.Adminstrator == this.user.ConnectedUser);

            return await ListStudents(university, student_state);
        }
        private async Task<List<UserSerializer>> GetList(List<UniversityStudent> source)
        {
            List<UserSerializer> list = new List<UserSerializer>();
            source.ForEach(async item =>
            {
                list.Add(await this.user.GetSerializer(item.User));
            });
            return list;
        }

        public async Task<UniversitySerializer> GetSerializer(int universit_id)
        {
            return await this.GetSerializer(
                await this.Get(universit_id));
        }

        public async Task<UniversitySerializer> GetSerializer(University university)
        {
            university.ImageCover = this.media.ConfigureUrl(university.ImageCover);
            return new UniversitySerializer
            {
                University = university,
                IsEnrolled = await CheckStudent(university) == null ? false : true,
                IsRequestSent=await CheckRequest(university,this.user.ConnectedUser)
            };
        }
        private async Task<UniversityStudent> CheckStudent(University univ,List<UniversityStudent> source=null)
        {
            if (source == null)
            {
                source = this.context.UniversitiesStudents
                    .Include(us=>us.User)
                    .Include(us=>us.University)
                    .ToList();
            }

            return source.FirstOrDefault(us => us.University == univ && us.User==this.user.ConnectedUser);
        }
        private async Task<bool> CheckRequest(University university,User user)
        {
            var checkjoin =  context.Requests
                .Include(x => x.User).Include(x => x.University)
                .FirstOrDefault(join => join.User == user && join.University == university);

            if (checkjoin != null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<RequestSerializer>> ListRequest()
        {
            var university = await this.context.Universities.Include(u => u.Adminstrator)
                .FirstOrDefaultAsync(u => u.Adminstrator == this.user.ConnectedUser);

            var requests=await this.context.Requests
                .Include(u=>u.University)
                .Include(u=>u.User)
                .Where(req=>req.University==university).ToListAsync();

            return await this.GetRequests(requests);
        }
        private async Task<List<RequestSerializer>> GetRequests(List<Request> requests)
        {
            List<RequestSerializer> serializers = new List<RequestSerializer>();
            requests.ForEach(async req =>
            {
                RequestSerializer serializer = new RequestSerializer
                {
                    Request = req,
                    User = await this.user.GetSerializer(req.User)
                };
                serializers.Add(serializer);
            });

            return serializers;
        }

        public async Task RejectRequest(int request_id)
        {
            var request = await this.context.Requests.FirstOrDefaultAsync(req => req.Id == request_id);

            this.context.Requests.Remove(request);

            await context.SaveChangesAsync();

            return;
        }

        public async Task<OneOf<string, UniversityStudent>> Graduate(int student_id)
        {
            var student_search = this.user.Get(student_id);
            var student = student_search.AsT1;

            var university = await this.context.Universities
                .Include(u=>u.Adminstrator)
                .FirstOrDefaultAsync(u => u.Adminstrator == this.user.ConnectedUser);

            var checkifstudent = await this.context.UniversitiesStudents
                .Include(us => us.University)
                .Include(us => us.User)
                .FirstOrDefaultAsync(
                us => us.User == student && us.University == university);

            if (checkifstudent==null)
            {
                return "You're not a student";
            }

            return await this.Graduate(checkifstudent);

        }

        public async Task<OneOf<string,UniversityStudent>> Graduate(UniversityStudent universityStudent)
        {
            var checkif = universityStudent.Date_quit.HasValue;
            if (checkif)
            {
                return "You are already graduate";
            }

            universityStudent.Date_quit = DateTime.Now;

            await this.context.SaveChangesAsync();

            return universityStudent;
        }

        public Task<string> Graduate()
        {
            throw new NotImplementedException();
        }

        public async Task<List<UniversitySerializer>> GetUserAcademicCaree()
        {
            return await this.GetUserAcademicCaree(this.user.ConnectedUser);
        }

        public async Task<List<UniversitySerializer>> GetUserAcademicCaree(User user)
        {
            var career = await this.context.UniversitiesStudents
                .Include(us => us.User)
                .Include(us => us.University)
                .Where(us => us.User == user)
                .Select(us=>us.University)
                .ToListAsync();

            return await this.GetList(career);
        }

        public async Task<List<UniversitySerializer>> GetUserAcademicCaree(int user)
        {
            return await this.GetUserAcademicCaree(
                this.user.Get(user).AsT1);
        }
    }
}
