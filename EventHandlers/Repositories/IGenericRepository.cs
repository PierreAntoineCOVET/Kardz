using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventHandlers.Repositories
{
    public interface IGenericRepository
    {
        Task Add<T>(T entity) where T : class;

        Task Update<T>(T entity) where T : class;

        Task<List<T>> Query<T>(ISpecification<T> spec) where T : class;

        Task<T> GetSingleOrDefault<T>(ISpecification<T> spec) where T : class;

        Task Delete<T>(T entity) where T : class;

        Task<int> SaveChanges();
    }
}
