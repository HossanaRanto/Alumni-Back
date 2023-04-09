using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Repository;
using Alumni_Back.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Back.Controllers
{
    [Route("message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IInterractorRepository _message;

        public MessageController(IInterractorRepository message)
        {
            _message = message;
        }

        [HttpPost("send")]
        [Authorization]
        public async Task<ActionResult<MessageSerializer>> Send([FromBody] MessageDTO message)
        {
            return await _message.SendMessage(message);
        }

        [HttpGet("{user_id}")]
        [Authorization]
        public async Task<ActionResult<List<MessageSerializer>>> GetMessages(
            [FromQuery] int? offset, [FromQuery] int? limit,[FromRoute] int user_id)
        {
            return await _message.GetMessages(user_id,offset,limit);
        }

        [HttpGet("last_interaction")]
        [Authorization]
        public async Task<ActionResult<List<LastInterractionSerializer>>> LastInteraction(
            [FromQuery] int? offset, [FromQuery] int? limit)
        {
            return await _message.GetLastInterractions(offset, limit);
        }

        [HttpGet("availables")]
        [Authorization]
        public async Task<ActionResult<List<User>>> GetList([FromQuery] int? offset,int? limit)
        {
            return await _message.InterractorAvailable(offset, limit);
        }
    }
}
