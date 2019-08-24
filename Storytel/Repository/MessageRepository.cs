using Microsoft.EntityFrameworkCore;
using Storytel.Models;
using Storytel.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Repository
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(StorytelContext context) : base(context){}

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            return await FindAll()
               .OrderBy(x => x.CreateDate)
               .ToListAsync();
        }


        public async Task<Message> GetMessageByIdAsync(int MessageId, int UserId)
        {
            return await FindByCondition(o => o.Id.Equals(MessageId) && o.UserId.Equals(UserId))
                .DefaultIfEmpty(new Message())
                .SingleAsync();
        }


        public async Task CreateMessageAsync(Message Message)
        {
            Create(Message);
            await SaveAsync();
        }

        public async Task UpdateMessageAsync( Message Message)
        {
            Update(Message);
            await SaveAsync();
        }

        public async Task DeleteMessageAsync(Message Message)
        {
            Delete(Message);
            await SaveAsync();
        }

    }
}
