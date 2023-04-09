using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Serializers;
using Alumni_Back.Validators;
using OneOf;
using OneOf.Types;

namespace Alumni_Back.Repository
{
    public interface IUniversityRepository
    {
        OneOf<University,ValidationFailed> Create(UniversityDto university);
        Task<University> Get(int university_id);
        Task<UniversitySerializer> GetSerializer(int universit_id);
        Task<UniversitySerializer> GetSerializer(University university);

        Task<List<UserSerializer>> ListStudents(string student_state);
        Task<List<UserSerializer>> ListStudents(int university_id,string student_state);
        Task<List<UserSerializer>> ListStudents(University university, string student_state);

        Task<OneOf<string, Request>> AsktoJoin(int university_id);
        OneOf<string, NotFound, ValidationFailed> JoiningValidation(int join_id);
        Task RejectRequest(int request_id);
        Task<List<RequestSerializer>> ListRequest();

        Task<List<Request>>  GetListRequest();
        Task<OneOf<string, UniversityStudent>> Graduate(int studentid);
        Task<OneOf<string, UniversityStudent>> Graduate(UniversityStudent universityStudent);
        Task<string> Graduate();

        bool checkUniversityname(string name);
        Task<List<UniversitySerializer>> GetUniversities();
        Task<List<UniversitySerializer>> GetUniversities(string name);
        Task<List<UniversitySerializer>> GetUserAcademicCaree();
        Task<List<UniversitySerializer>> GetUserAcademicCaree(User user);
        Task<List<UniversitySerializer>> GetUserAcademicCaree(int user);

    }
}
