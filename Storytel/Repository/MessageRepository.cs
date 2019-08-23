using Storytel.Models;
using Storytel.Repository.Interface;

namespace Storytel.Repository
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(StorytelContext context) : base(context)
        {
        }
    }
}
