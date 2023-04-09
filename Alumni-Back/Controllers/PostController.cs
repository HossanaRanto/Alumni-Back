using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Repository;
using Alumni_Back.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alumni_Back.Controllers
{
    [Route("post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository post;

        public PostController(IPostRepository post)
        {
            this.post = post;
        }

        [HttpPost]
        [Authorization]
        public async Task<ActionResult<Post>> Post([FromBody] PostDto post)
        {
            var result = await this.post.Post(post);
            return result.Match<ActionResult>(
                _ => BadRequest(new
                {
                    message = _
                }),
                p => Ok(p));
        }

        [HttpPost("update_photo/{post_id}")]
        [Authorization]
        public async Task<ActionResult> UpdatePicture([FromRoute] int post_id, [FromForm] FileUpload file)
        {
            var result =await this.post.UpdateImagePost(file, post_id);

            //return Ok(new {message=result});

            return result.Match<ActionResult>(
                _ => BadRequest(new { message = _ }),
                p => Ok(p));
        }

        [HttpGet("random")]
        [Authorization]
        public async Task<ActionResult<List<PostSerializer>>> GetPosts([FromQuery] int? offset,int? limit)
        {
            //return offset;
            return await post.GetPostRandom(offset, limit);
        }

        [HttpGet("commentaries/{post_id}")]
        [Authorization]
        public async Task<ActionResult<List<CommentSerializer>>> GetCommentaries([FromRoute] int post_id)
        {
            return await post.GetCommentaries(post_id);
        }

        [HttpPost("comment/{post_id}")]
        [Authorization]
        public async Task<ActionResult<CommentSerializer>> Comment([FromRoute] int post_id, [FromBody] CommentDTO comment)
        {
            var comment_obj= await post.Comment(post_id, comment.Content);
            return Ok(new CommentSerializer
            {
                Commentator = comment_obj.Commentator,
                Content = comment_obj.Content
            });
        }

        [HttpPost("like/{post_id}")]
        [Authorization]
        public async Task<ActionResult<LikeSerializer>> Like([FromRoute] int post_id)
        {
            var result = await post.Like(post_id);
            return result.Match<ActionResult>(
                _ => BadRequest(new { Error = _ }),
                isliked => Ok(new LikeSerializer
                {
                    Liked = isliked
                })
                );
        }
    }
}
