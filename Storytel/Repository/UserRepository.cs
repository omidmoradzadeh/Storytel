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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(StorytelContext context) : base(context) { }


        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await FindAll()
                                .OrderBy(x => x.Id)
                                .ToListAsync();
        }


        public async Task<IEnumerable<UserDetailVM>> GetAllUserWithDetailAsync()
        {
            return await FindAll()
                               .Select(x => new UserDetailVM()
                               {
                                   Id = x.Id,
                                   Name = x.Name,
                                   Family = x.Family,
                                   Email = x.Email,
                                   UserName = x.UserName,
                                   IsAdmin = x.IsAdmin
                               })
                               .OrderBy(x => x.Id)
                               .ToListAsync();
        }


        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await FindByCondition(o => o.Id.Equals(userId))
                .DefaultIfEmpty(new User())
                .SingleAsync();
        }


        public async Task<UserDetailVM> GetUserWithDetailByIdAsync(int userId)
        {
            return await FindByCondition(o => o.Id.Equals(userId))
                           .Select(x => new UserDetailVM() {
                               Id = x.Id,
                               Name = x.Name,
                               Family = x.Family,
                               Email = x.Email,
                               UserName = x.UserName,
                               IsAdmin = x.IsAdmin
                           })
                           .DefaultIfEmpty(new UserDetailVM())
                           .SingleAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            Create(user);
            await SaveAsync();
        }

        public async Task<int> CreateUserAsync(User dbUser, UserAddDTO user)
        {
            Create(dbUser.MapForAdd(user));
            await SaveAsync();
            return dbUser.Id;
        }

        public async Task UpdateUserAsync(User user)
        {
            Update(user);
            await SaveAsync();
        }

        public async Task UpdateUserAsync(User dbUser, UserEditDTO user)
        {
            dbUser.MapForEdit(user);
            Update(dbUser);
            await SaveAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            Delete(user);
            await SaveAsync();
        }

        
    }


}
