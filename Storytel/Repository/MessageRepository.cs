using Microsoft.EntityFrameworkCore;
using Storytel.Models;
using Storytel.Models.DTO;
using Storytel.Models.Extensions;
using Storytel.Models.VM;
using Storytel.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Repository
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(StorytelContext context) : base(context) { }


        public async Task<IEnumerable<Message>> GetAllMessageAsync(int userId)
        {
            return await FindByCondition(user=>user.UserId.Equals(userId))
                                .OrderBy(x => x.Id)
                                .ToListAsync();
        }


        public async Task<IEnumerable<MessageDetailVM>> GetAllMessageWithDetailAsync(int userId)
        {
            return await FindByCondition(user => user.UserId.Equals(userId))
                               .Select(x => new MessageDetailVM()
                               {
                                   Id = x.Id,
                                   Text = x.Text,
                                   UserName = x.User.UserName,
                                   CreateDate = x.CreateDate
                               })
                               .OrderBy(x => x.Id)
                               .ToListAsync();
        }


        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await FindByCondition(o => o.Id.Equals(messageId))
                .DefaultIfEmpty(new Message())
                .SingleAsync();
        }


        public async Task<MessageDetailVM> GetMessageWithDetailByIdAsync(int userId)
        {
            return await FindByCondition(o => o.Id.Equals(userId))
                           .Select(x => new MessageDetailVM()
                           {
                               Id = x.Id,
                               Text = x.Text,
                               UserName = x.User.UserName,
                               CreateDate = x.CreateDate
                           })
                           .DefaultIfEmpty(new MessageDetailVM())
                           .SingleAsync();
        }

        public async Task CreateMessageAsync(Message message)
        {
            Create(message);
            await SaveAsync();
        }

        public async Task<int> CreateMessageAsync(Message dbMessage, MessageAddDTO message , int userId)
        {
            Create(dbMessage.MapForAdd(message,userId));
            await SaveAsync();
            return dbMessage.Id;
        }

        public async Task UpdateMessageAsync(Message message)
        {
            Update(message);
            await SaveAsync();
        }

        public async Task UpdateMessageAsync(Message dbMessage, MessageEditDTO message)
        {
            dbMessage.MapForEdit(message);
            Update(dbMessage);
            await SaveAsync();
        }

        public async Task DeleteMessageAsync(Message message)
        {
            Delete(message);
            await SaveAsync();
        }

        
    }
}
