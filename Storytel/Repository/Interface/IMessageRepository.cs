using Storytel.Models;
using Storytel.Models.DTO;
using Storytel.Models.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Storytel.Repository.Interface
{
    public interface IMessageRepository :  IRepositoryBase<Message>
    {
        Task<IEnumerable<Message>> GetAllMessageAsync(int userId);
        Task<IEnumerable<MessageDetailVM>> GetAllMessageWithDetailAsync(int userId);
        Task<Message> GetMessageByIdAsync(int messageId);
        Task<MessageDetailVM> GetMessageWithDetailByIdAsync(int messageId);
        Task CreateMessageAsync(Message message);
        Task<int> CreateMessageAsync(Message dbMessage, MessageAddDTO message , int userId);
        Task UpdateMessageAsync(Message message);
        Task UpdateMessageAsync(Message dbMessage, MessageEditDTO message );
        Task DeleteMessageAsync(Message message);
    }
}
