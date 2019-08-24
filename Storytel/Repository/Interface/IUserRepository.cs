using Storytel.Models;
using Storytel.Models.DTO;
using Storytel.Models.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Storytel.Repository.Interface
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<IEnumerable<UserDetailVM>> GetAllUserWithDetailAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<UserDetailVM> GetUserWithDetailByIdAsync(int userId);
        Task CreateUserAsync(User user);
        Task<int> CreateUserAsync(User dbUser, UserAddDTO user);
        Task UpdateUserAsync(User user);
        Task UpdateUserAsync(User dbUser, UserEditDTO user);
        Task DeleteUserAsync(User user);
    }
}
