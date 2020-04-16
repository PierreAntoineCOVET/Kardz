using Microsoft.EntityFrameworkCore;
using Repositories.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ReadRepositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ReadDbContext ReadDbContext;

        public GenericRepository(ReadDbContext readDbContext)
        {
            ReadDbContext = readDbContext;
        }

        public Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return ReadDbContext.Set<T>().AnyAsync(predicate);
        }

        public Task Delete(T entity)
        {
            ReadDbContext.Set<T>().Remove(entity);

            return Task.CompletedTask;
        }

        public Task<T> Get(object[] keys)
        {
            return ReadDbContext.Set<T>().FindAsync(keys).AsTask();
        }

        public Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return ReadDbContext.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return ReadDbContext.Set<T>().AsQueryable();
        }

        public Task Add(T entity)
        {
            return ReadDbContext.Set<T>().AddAsync(entity).AsTask();
        }

        public Task Update(T entity)
        {
            ReadDbContext.Set<T>().Update(entity);

            return Task.CompletedTask;
        }

        public Task<int> SaveChanges()
        {
            return ReadDbContext.SaveChangesAsync();
        }
    }
}
