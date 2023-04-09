using Alumni_Back.Models;

namespace Alumni_Back.Repository
{
    public interface IPointRepository
    {
        Task AddPoint(User user,int point);
        Task<int> GetPoint(User user);
    }
}
