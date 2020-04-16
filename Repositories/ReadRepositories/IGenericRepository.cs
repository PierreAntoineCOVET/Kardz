using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ReadRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);

        Task Update(T entity);

        Task<T> Get(Expression<Func<T, bool>> predicate);
        
        Task<T> Get(params object[] keys);

        IQueryable<T> GetAll();

        Task Delete(T entity);

        Task<bool> Any(Expression<Func<T, bool>> predicate);

        Task<int> SaveChanges();
    }
}
