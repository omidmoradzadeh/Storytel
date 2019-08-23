using Storytel.Models;
using Storytel.Repository.Interface;

namespace Storytel.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private StorytelContext _context;
        private IUserRepository _user;
        private IMessageRepository _message;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }

                return _user;
            }
        }

        public IMessageRepository Message
        {
            get
            {
                if (_message == null)
                {
                    _message = new MessageRepository(_context);
                }

                return _message;
            }
        }

        public RepositoryWrapper(StorytelContext repositoryContext)
        {
            _context = repositoryContext;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
