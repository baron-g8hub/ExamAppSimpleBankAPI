using DataAccessLayer.DataContextEFCore;
using Microsoft.EntityFrameworkCore;
using SimpleBankWebAPI.Contracts;
using System.Linq.Expressions;

namespace SimpleBankWebAPI.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDBContext Repository { get; set; }
        public RepositoryBase(ApplicationDBContext repositoryContext)
        {
            Repository = repositoryContext;
        }

        //public IQueryable<T> FindAll() => RepositoryContext.Set<T>().AsNoTracking();
        //public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
        //    RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        //public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
        //public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
        //public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
    }
}
