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
    public class GenericRepository : IGenericRepository
    {
        private ReadDbContext ReadDbContext;

        public GenericRepository(ReadDbContext readDbContext)
        {
            ReadDbContext = readDbContext;
        }

        public Task Delete<T>(T entity)
             where T : class
        {
            ReadDbContext.Set<T>().Remove(entity);

            return Task.CompletedTask;
        }

        public IQueryable<T> Query<T>()
             where T : class
        {
            return ReadDbContext.Set<T>().AsQueryable();
        }

        public Task Add<T>(T entity)
             where T : class
        {
            return ReadDbContext.Set<T>().AddAsync(entity).AsTask();
        }

        public Task Update<T>(T entity)
             where T : class
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
