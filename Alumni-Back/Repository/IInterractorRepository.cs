using Alumni_Back.DTO;
using Alumni_Back.Models;
using Alumni_Back.Serializers;

namespace Alumni_Back.Repository
{
    public interface IInterractorRepository
    {
        Task<List<User>> InterractorAvailable(int? offset, int? limit);
        Task<MessageSerializer> SendMessage(MessageDTO message);
        Task<List<MessageSerializer>> GetMessages(User user,int? offset, int? limit);
        Task<List<MessageSerializer>> GetMessages(int user,int? offset,int? limit);
        Task<List<LastInterractionSerializer>> GetLastInterractions(int? offset, int? limit);
    }
}
