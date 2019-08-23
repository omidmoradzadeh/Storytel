using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Repository.Interface
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IMessageRepository Message { get; }
        void Save();
    }
}
