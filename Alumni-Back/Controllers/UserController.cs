using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Repository;
using Alumni_Back.Serializers;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Back.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userService;
        public UserController(IUserRepository userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto auth)
        {
            var result=await _userService.Login(auth);
            return result.Match<IActionResult>(
                _=>BadRequest(new { Error = _ }),
                token=>Ok(token));
        }
        [HttpGet("refresh_token/{token}")]
        public async Task<ActionResult<Token>> RefreshToken([FromRoute] string token)
        {
            var result = await _userService.RefreshToken(token);
            return result.Match<ActionResult>(
                _=>BadRequest(new {Error=_}),
                token=>Ok(token));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto user)
        {
            var result = _userService.Create(user);
            //return this.RedirectToAction("Login", "User");

            return result.Match<IActionResult>(
                u => Ok(u),
                error => BadRequest(error));
        }
        [HttpPost("update")]
        [Authorization]
        public async Task<IActionResult> Update([FromBody] UserDto user, [FromQuery] bool updatepassword)
        {
            var result = _userService.Update(user,updatepassword);
            //return this.RedirectToAction("Login", "User");

            return result.Match<IActionResult>(
                u => Ok(u),
                error => BadRequest(error));
        }

        [HttpGet("checkusername/{username}")]
        public async Task<IActionResult> CheckUsername([FromRoute] string username)
        {
            return Ok(await _userService.CheckUsername(username,true));
        }
        [HttpGet("checkmyusername/{username}")]
        [Authorization]
        public async Task<IActionResult> CheckMyUsername([FromRoute] string username)
        {
            return Ok(await _userService.CheckUsername(username, false));
        }
        [HttpGet("checkemail/{email}")]
        public async Task<IActionResult> CheckEmail([FromRoute] string email)
        {
            return Ok(await _userService.CheckEmail(email,true) );
        }
        [HttpGet("checkmyemail/{email}")]
        [Authorization]
        public async Task<IActionResult> CheckMyEmail([FromRoute] string email)
        {
            return Ok(await _userService.CheckEmail(email, false));
        }

        [HttpGet("role")]
        [Authorization]
        public async Task<IActionResult> Role()
        {
            return Ok(new { role = _userService.GetRole(_userService.ConnectedUser) });
        }

        [HttpPost("uploadimage/{mediatype}")]
        [Authorization]
        public async Task<IActionResult> Upload([FromForm] FileUpload uploadImage, [FromRoute] string mediatype)
        {
            await _userService.Upload(uploadImage, MediaType.PDP);
            return Ok(new
            {
                message = "Image uploaded"
            });
        }

        [HttpGet("pictures")]
        [Authorization]
        public async Task<IActionResult> GetPictures([FromQuery] int? offset, [FromQuery] int? limit)
        {
            return Ok(new
            {
                pictures = await this._userService.GetPictures(offset,limit)
            });
        }

        [HttpGet("pictures/{user}")]
        [Authorization]
        public async Task<IActionResult> GetPictures([FromRoute] int user, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            return Ok(new
            {
                pictures = await this._userService.GetPictures(user,offset,limit)
            });
        }

        [HttpGet]
        [Authorization]
        public async Task<ActionResult<UserSerializer>> Get()
        {
            return await this._userService.GetSerializer();
        }

        [HttpGet("{user}")]
        [Authorization]
        public async Task<ActionResult<UserSerializer>> Get([FromRoute] int user)
        {
            return await this._userService.GetSerializer(user);
        }

        [HttpGet("test")]
        [Authorization]
        public async Task<IActionResult> Test()
        {
            return Ok(new {message="my test"});
        }
    }
}
