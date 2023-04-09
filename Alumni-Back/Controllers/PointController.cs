using Alumni_Back.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Back.Controllers
{
    [Route("point")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IPointRepository point;
        private readonly IUserRepository user;

        public PointController(IPointRepository point, IUserRepository user)
        {
            this.point = point;
            this.user = user;
        }


        [HttpGet]
        [Authorization]
        public async Task<IActionResult> Get()
        {
            return Ok(new
            {
                value= await point.GetPoint(user.ConnectedUser)
            });
        }
    }
}
