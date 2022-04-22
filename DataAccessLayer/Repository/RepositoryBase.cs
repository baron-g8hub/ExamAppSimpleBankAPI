using DataAccessLayer.DataContextEFCore;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Contracts;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDBContext _context { get; set; }
        public RepositoryBase(ApplicationDBContext dbcontext)
        {
            _context = dbcontext;
        }

        //public IQueryable<T> FindAll() => RepositoryContext.Set<T>().AsNoTracking();
        //public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
        //    RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        //public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
        //public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
        //public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
    }
}
