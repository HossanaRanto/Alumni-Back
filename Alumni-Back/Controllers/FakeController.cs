using Alumni_Back.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Back.Controllers
{
    [Route("faker")]
    [ApiController]
    public class FakeController : ControllerBase
    {
        private readonly IFaker _faker;

        public FakeController(IFaker faker)
        {
            _faker = faker;
        }

        [HttpPost("user")]
        public async Task<IActionResult> FakeUser()
        {
            await _faker.GenerateUser();
            return Ok(new
            {
                message = "Fake data generated"
            });
        }
    }
}
