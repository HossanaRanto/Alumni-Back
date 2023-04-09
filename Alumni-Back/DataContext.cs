using Alumni_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Back
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<UniversityStudent> UniversitiesStudents { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Refreshtoken> Refreshtokens { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}
