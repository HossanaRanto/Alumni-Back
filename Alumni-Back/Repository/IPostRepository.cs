using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Serializers;
using OneOf;

namespace Alumni_Back.Repository
{
    public interface IPostRepository
    {
        Task<Post> GetPost(int post_id);
        Task<OneOf<string,Post>> Post(PostDto post);
        Task<OneOf<string, Post>> UpdateImagePost(FileUpload file, int post_id);
        Task<OneOf<string, Post>> UpdateImagePost(FileUpload file,Post post);
        Task<Commentary> Comment(int post_id,string content);
        Task<Commentary> Comment(Post post,string content);
        Task<List<CommentSerializer>> GetCommentaries(int post_id);
        Task<List<CommentSerializer>> GetCommentaries(Post post);
        Task<OneOf<string,bool>> Like(int post_id);
        Task<OneOf<string, bool>> Like(Post post);
        Task<bool> CheckIfLiked(int post_id);
        Task<bool> CheckIfLiked(Post post);
        Task<bool> CheckIfCommented(int post_id);
        Task<bool> CheckIfCommented(Post post);
        Task<int> CountLikes(int post_id);
        Task<int> CountLikes(Post post);
        Task<List<PostSerializer>> GetPostRandom(int? offset, int? limit);
        void GetMyPost(int? offset, int? limit);
    }
}
