using Storytel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Storytel.Repository.Interface
{
    public interface IMessageRepository :  IRepositoryBase<Message>
    {
        Task<IEnumerable<Message>> GetAllMessagesAsync();
        Task<Message> GetMessageByIdAsync(int messsageId , int UserId);
        Task CreateMessageAsync(Message message);
        Task UpdateMessageAsync(Message message);
        Task DeleteMessageAsync(Message message);
    }
}
