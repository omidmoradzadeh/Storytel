using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Storytel.Repository.Interface
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T> Find(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}
