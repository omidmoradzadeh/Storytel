using Storytel.Models;
using Storytel.Repository.Interface;

namespace Storytel.Repository
{
    public class UserRepository :  RepositoryBase<User>, IUserRepository
    {
        public UserRepository(StorytelContext context): base(context)
        {
        }
    }
}
