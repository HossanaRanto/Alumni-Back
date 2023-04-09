using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Repository;
using Alumni_Back.Serializers;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Alumni_Back.Services
{
    public class PostService : IPostRepository
    {
        private readonly DataContext context;
        private readonly IPointRepository _point;
        private readonly IUserRepository _user;
        private readonly IMediaRepository _media;

        public PostService(DataContext context, IPointRepository point, IUserRepository user, IMediaRepository media)
        {
            this.context = context;
            _point = point;
            _user = user;
            _media = media;
        }

        public Task<bool> CheckIfCommented(int post_id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckIfCommented(Post post)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckIfLiked(int post_id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckIfLiked(Post post)
        {
            var checkif = this.context.Likes.Include(like => like.Liker)
                .FirstOrDefault(like => like.Liker == this._user.ConnectedUser);

            return checkif==null?false:true;
        }

        public async Task<Commentary> Comment(int post_id,string content)
        {
            return await this.Comment(
                await this.GetPost(post_id),
                content);
        }

        public async Task<Commentary> Comment(Post post,string content)
        {
            var comment = new Commentary
            {
                Commentator = this._user.ConnectedUser,
                Content = content,
                Post = post
            };

            this.context.Commentaries.Add(comment);

            await this.context.SaveChangesAsync();

            return comment;

        }

        public Task<int> CountLikes(int post_id)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountLikes(Post post)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CommentSerializer>> GetCommentaries(int post_id)
        {
            return await this.GetCommentaries(
                await this.GetPost(post_id));
        }

        public async Task<List<CommentSerializer>> GetCommentaries(Post post)
        {
            return await this.context.Commentaries.Include(comment => comment.Post)
                .Where(comment => comment.Post == post)
                .Select(comment => new CommentSerializer
                {
                    Commentator = comment.Commentator,
                    Content = comment.Content
                }).ToListAsync();
        }

        public void GetMyPost(int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> GetPost(int post_id)
        {
            return await this.context.Posts.Include(post=>post.Publisher).FirstOrDefaultAsync(p => p.Id == post_id);
        }

        public async Task<List<PostSerializer>> GetPostRandom(int? offset, int? limit)
        {
            var posts = await this.context.Posts.Include(p => p.Publisher)
                .Where(p => p.Publisher != _user.ConnectedUser).ToListAsync();
            if(offset.HasValue && limit.HasValue)
            {
                posts = posts.Skip(offset.Value).Take(limit.Value).ToList();
            }

            return await this.GetList(posts);
                
        }
        private async Task<List<PostSerializer>> GetList(List<Post> posts)
        {
            List<PostSerializer> list = new List<PostSerializer>();
            posts.ForEach(async post =>
            {
                post.ImagePath = this._media.ConfigureUrl(post.ImagePath);
                var postSerializer = new PostSerializer
                {
                    Post = post,
                    IsLiked = await this.CheckIfLiked(post)
                };
                list.Add(postSerializer);
            });

            return list;
        }
        public async Task<OneOf<string, bool>> Like(int post_id)
        {
            return await this.Like(await this.GetPost(post_id));
        }

        public async Task<OneOf<string, bool>> Like(Post post)
        {
            if (post == null)
            {
                return "Post not found";
            }
            if (post.Publisher == this._user.ConnectedUser)
            {
                return "Cannot perform liking";
            }

            Models.Like like = new Like
            {
                Liker = this._user.ConnectedUser,
                Post = post
            };

            await this.context.Likes.AddAsync(like);
            await _point.AddPoint(post.Publisher, PointValue.FromReaction);


            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<OneOf<string, Post>> Post(PostDto postdto)
        {
            var checkpoint =await this._point.GetPoint(this._user.ConnectedUser);
            //if (checkpoint < PointConstraint.ForPost)
            //{
            //    return "You don't have enough point";
            //}

            Post post = new Post
            {
                Publisher=this._user.ConnectedUser,
                Content = postdto.Content
            };

            this.context.Posts.Add(post);
            await this.context.SaveChangesAsync();

            return post;
        }

        public async Task<OneOf<string, Post>> UpdateImagePost(FileUpload file,int post_id)
        {
            return await this.UpdateImagePost(file, await this.GetPost(post_id));
        }
        public async Task<OneOf<string,Post>> UpdateImagePost(FileUpload file,Post post)
        {
            if (post == null)
            {
                return "Post not found";
            }
            if (post.Publisher != this._user.ConnectedUser)
            {
                return "You are not the publisher of that post";
            }
            post.ImagePath = Guid.NewGuid() + Path.GetExtension(file?.File.FileName);
            await _media.Upload(file, post.ImagePath);

            this.context.Update<Post>(post);
            await this.context.SaveChangesAsync();

            return post;
        }
    }
}
