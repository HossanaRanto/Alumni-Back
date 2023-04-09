using Alumni_Back.DTO;
using Alumni_Back.Helpers;
using Alumni_Back.Models;
using Alumni_Back.Repository;
using Alumni_Back.Serializers;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Back.Services
{
    public class InterractionService : IInterractorRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IPointRepository _point;
        private readonly DataContext context;
        public InterractionService(DataContext context, IUserRepository userRepository, IPointRepository point)
        {
            _userRepository = userRepository;
            this.context = context;
            _point = point;
        }

        public async Task<List<LastInterractionSerializer>> GetLastInterractions(int? offset, int? limit)
        {
            var messages = await this.context.Messages.Include(m => m.Sender).Include(m => m.Receiver)
                .OrderByDescending(m => m.DateSend)
                .ToListAsync();

            var message_grouped = messages
                .GroupBy(m => m.Sender == this._userRepository.ConnectedUser ? m.Receiver : m.Sender)
                .Where(m=>m.Key!=this._userRepository.ConnectedUser)
                .ToList();
            //var message_if_sender = await messages
            //    .Where(m => m.Sender == _userRepository.ConnectedUser || m.Receiver==_userRepository.ConnectedUser).ToListAsync();
            //var message_if_receiver = await messages
            //    .Where(m => m.Receiver == _userRepository.ConnectedUser).ToListAsync();

            //var message_if_sender_group = message_if_sender.GroupBy(m => m.Receiver);
            //var message_if_receiver_group = message_if_receiver.GroupBy(m => m.Sender);

            //List<LastInterractionSerializer> messages_serialized= new List<LastInterractionSerializer>();

            //message_if_sender.ForEach(async sended =>
            //{
            //    var checkif_last = message_if_receiver_group.FirstOrDefault(mg =>mg.Key==sended.Receiver && mg.First().DateSend < sended.DateSend);
            //    if (checkif_last == null)
            //    {
            //        messages_serialized.Add(new()
            //        {
            //            User = await _userRepository.GetSerializer(sended.Receiver),
            //            LastMessage = sended.Content
            //        });
            //    }
            //    else
            //    {
            //        messages_serialized.Add(new()
            //        {
            //            User = await _userRepository.GetSerializer(checkif_last.Key),
            //            LastMessage = checkif_last.First().Content
            //        });
            //    }
            //});

            //var message_merge = message_if_sender_group.Concat(message_if_receiver_group)
            //    .Distinct();

            if (offset.HasValue && limit.HasValue)
            {
                message_grouped = message_grouped.Skip(offset.Value).Take(limit.Value).ToList();
            }
            return await GetList(message_grouped);

            //return messages_serialized;
        }
        private async Task<List<LastInterractionSerializer>> GetList(IEnumerable<IGrouping<User,Message>> source)
        {
            List<LastInterractionSerializer> list = new List<LastInterractionSerializer>();
            foreach(var item in source)
            {
                list.Add(new LastInterractionSerializer
                {
                    User = await _userRepository.GetSerializer(item.Key),
                    LastMessage = item.First().Content
                });
            }
            return list;
        }
        public async Task<List<MessageSerializer>> GetMessages(User user, int? offset, int? limit)
        {
            var messageses = await this.context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(message =>
                (message.Sender == user && message.Receiver==this._userRepository.ConnectedUser) || 
                (message.Receiver == user && message.Sender==this._userRepository.ConnectedUser))
                .OrderByDescending(message => message.DateSend)
                .ToListAsync();

            //var real_messages = messageses.Where(m =>
            //{
            //    if (m.isSender && m.message.Receiver == this._userRepository.ConnectedUser)
            //    {
            //        return true;
            //    }
            //    else if (!m.isSender && m.message.Sender == this._userRepository.ConnectedUser)
            //    {
            //        return true;
            //    }
            //    return false;
            //})
            //    .Select(m => m.message);

            if(offset.HasValue && limit.HasValue)
            {
                messageses = messageses
                    .Skip(offset.Value)
                    .Take(limit.Value).ToList();
                    
            }
            return messageses
                .Select(message =>
                new MessageSerializer(message, _userRepository.ConnectedUser))
                .ToList();

        }

        public async Task<List<MessageSerializer>> GetMessages(int user_id,int? offset, int? limit)
        {
            var user = this._userRepository.Get(user_id).AsT1;
            return await this.GetMessages(user,offset,limit);

        }

        public async Task<List<User>> InterractorAvailable(int? offset, int? limit)
        {
            //var messages = await this.context.Messages.Include(m => m.Sender)
            //    .Where(m => m.Sender == this._userRepository.ConnectedUser)
            //    .ToListAsync();

            //var available_intarrator=await this.context.Users
            //    .Where(u=>u!=_userRepository.ConnectedUser && _userRepository.GetRole(u)!=UserRole.Admin)
            //    .ToListAsync();

            var users = await this.context.Users
                .Where(u=>u!=this._userRepository.ConnectedUser)
                .ToListAsync();

            var messages = await GetLastInterractions(null, null);

            var available = users.Where(u => !messages.Any(m => m.User.User == u));

            if(offset.HasValue && limit.HasValue)
            {
                available = available.Skip(offset.Value).Take(limit.Value);
            }


            return available.ToList();
            
        }

        public async Task<MessageSerializer> SendMessage(MessageDTO data)
        {
            var to_user_search = this._userRepository.Get(data.To);
            User to_user = to_user_search.AsT1;

            if (to_user == null)
            {
                return null;
            }
            var message = new Message
            {
                Sender = this._userRepository.ConnectedUser,
                Receiver = to_user,
                Content = data.Content
            };

            await this.context.Messages.AddAsync(message);
            await this.context.SaveChangesAsync();

            if (_userRepository.GetRole(to_user) == UserRole.Alumni)
            {
                await _point.AddPoint(to_user, PointValue.FromMessage);
            }

            return new MessageSerializer(message,this._userRepository.ConnectedUser);
        }

    }
}
