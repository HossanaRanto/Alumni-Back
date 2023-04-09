using Alumni_Back.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Back.Controllers
{
    [Route("")]
    public class DefaultController : Controller
    {
        private readonly IMediaRepository media;

        public DefaultController(IMediaRepository media)
        {
            this.media = media;
        }

        [HttpGet("{path}")]
        public async Task<IActionResult> GetFile([FromRoute] string path)
        {
            return File(await this.media.Load(path),"image/*");
        }
    }
}
