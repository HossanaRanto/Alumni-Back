using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Repository;
using Alumni_Back.Serializers;
using Alumni_Back.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Back.Controllers
{
    [Route("university")]
    public class UniversityController : Controller
    {
        private readonly IUniversityRepository university;
        public UniversityController(IUniversityRepository university)
        {
            this.university = university;
        }

        [HttpPost]
        [Authorization]
        public async Task<IActionResult> Create([FromBody] UniversityDto university)
        {
            var result = this.university.Create(university);
            return result.Match<IActionResult>(
                u => Ok(u),
                errors => BadRequest(errors.Errorsmessages));
        }

        [HttpPost("updateimage")]
        [Authorization]
        public async Task<IActionResult> UpdatePicture([FromForm] FileUpload file)
        {
            var result = await this.university.ChangeImage(file);

            return result.Match<IActionResult>(
                _ => BadRequest(new { Error = _ }),
                u => Ok(u));
        }
        [HttpPost("join/{university_id}")]
        [Authorization]
        public async Task<IActionResult> Join([FromRoute] int university_id)
        {
            var result = await this.university.AsktoJoin(university_id);
            return result.Match<IActionResult>(
                _ => BadRequest(new { Error = _ }),
                join => Ok(join));
        }

        [HttpPost("join/accept/{join_id}")]
        [Authorization(Role =UserRole.Admin)]
        public async Task<IActionResult> Accept([FromRoute] int join_id)
        {
            var result = this.university.JoiningValidation(join_id);
            return result.Match <IActionResult>(
                _ => Ok(new { Error = _ }),
                _ => BadRequest(new {Error="Your request to join that university was not found"}),
                error => BadRequest(error.Errorsmessages));
        }

        [HttpPost("join/reject/{join_id}")]
        [Authorization(Role = UserRole.Admin)]
        public async Task<IActionResult> Reject([FromRoute] int join_id)
        {
            await this.university.RejectRequest(join_id);

            return Ok(new { message = "Request rejected" }); 
        }

        [HttpPost("graduate/{student_id}")]
        [Authorization(Role =UserRole.Admin)]
        public async Task<ActionResult> Graduate([FromRoute] int student_id)
        {
            var graduate = await this.university.Graduate(student_id);
            return graduate.Match<ActionResult>(
                _ => BadRequest(new { message = _ }),
                g => Ok(new { isGraduate=true}));
        }

        [HttpGet("checkname/{name}")]
        public async Task<IActionResult> CheckName([FromRoute] string name)
        {
            return Ok(new { IsExist = university.checkUniversityname(name) });
        }

        //[HttpGet("request")]
        //[Authorization(Role ="admin")]
        //public async Task<ActionResult<List<Request>>> GetRequests()
        //{
        //    return Ok(await university.GetListRequest());
        //}

        [HttpGet("list")]
        [Authorization]
        public async Task<ActionResult<List<UniversitySerializer>>> GetList([FromQuery] string? name)
        {
            return await university.GetUniversities(name);
        }

        [HttpGet("student/{university}/{state}")]
        [Authorization]
        public async Task<ActionResult<List<UserSerializer>>> GetStudents([FromRoute] int university,[FromRoute] string state)
        {
            return await this.university.ListStudents(university, state);
        }
        [HttpGet("student/{state}")]
        [Authorization(Role =UserRole.Admin)]
        public async Task<ActionResult<List<UserSerializer>>> GetStudents([FromRoute] string state)
        {
            return await this.university.ListStudents(state);
        }
        [HttpGet("{id}")]
        [Authorization]
        public async Task<ActionResult<UniversitySerializer>> Get([FromRoute] int id)
        {
            return await this.university.GetSerializer(id);
        }

        [HttpGet("requests")]
        [Authorization(Role =UserRole.Admin)]
        public async Task<ActionResult<List<RequestSerializer>>> GetRequests()
        {
            return await this.university.ListRequest();
        }

        [HttpGet("academic_career")]
        [Authorization]
        public async Task<ActionResult<List<UniversitySerializer>>> GetAcademicCareers()
        {
            return await this.university.GetUserAcademicCaree();
        }
        [HttpGet("academic_career/{user}")]
        [Authorization]
        public async Task<ActionResult<List<UniversitySerializer>>> GetAcademicCareers([FromRoute] int user)
        {
            return await this.university.GetUserAcademicCaree(user);
        }
    }
}
