using Alumni_Back.Models;
using Alumni_Back.Repository;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Back.Services
{
    public class PointService : IPointRepository
    {
        private readonly DataContext context;

        public PointService(DataContext context)
        {
            this.context = context;
        }

        public async Task AddPoint(User user, int value)
        {
            var point = await this.context.Points.Include(p => p.User).FirstOrDefaultAsync(p => p.User == user);
            if (point == null)
            {
                point = new Point
                {
                    User = user
                };
                await this.context.Points.AddAsync(point);
            }
            point.Value += value;

            await this.context.SaveChangesAsync();
        }

        public async Task<int> GetPoint(User user)
        {
            var point = await this.context.Points.Include(p => p.User).FirstOrDefaultAsync(p => p.User == user);
            int point_value = 0;
            if(point!=null)
            {
                point_value = point.Value;
            }
            return point_value;
        }
    }
}
