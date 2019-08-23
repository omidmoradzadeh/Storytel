using Microsoft.EntityFrameworkCore;
using Storytel.Models;
using Storytel.Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Storytel.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected StorytelContext Context { get; set; }

        public RepositoryBase(StorytelContext repositoryContext)
        {
            Context = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }
    }
}
