using EventHandlers.Repositories;
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
    /// <summary>
    /// Generic read modele repository.
    /// </summary>
    public class GenericRepository : IGenericRepository
    {
        /// <summary>
        /// Ef core Db Context.
        /// </summary>
        private ReadDbContext ReadDbContext;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="readDbContext">Db context.</param>
        public GenericRepository(ReadDbContext readDbContext)
        {
            ReadDbContext = readDbContext;
        }

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <typeparam name="T">Entity's type.</typeparam>
        /// <param name="entity">Entity.</param>
        /// <returns></returns>
        public Task Delete<T>(T entity)
             where T : class
        {
            ReadDbContext.Set<T>().Remove(entity);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Query entities.
        /// </summary>
        /// <typeparam name="T">Entity's type.</typeparam>
        /// <returns><see cref="IQueryable{T}"/>.</returns>
        public Task<List<T>> Query<T>(ISpecification<T> spec)
             where T : class
        {
            return ApplySpecification(spec).ToListAsync();
        }

        /// <summary>
        /// Add an entity.
        /// </summary>
        /// <typeparam name="T">Entity's type.</typeparam>
        /// <param name="entity">Entity.</param>
        /// <returns></returns>
        public Task Add<T>(T entity)
             where T : class
        {
            return ReadDbContext.Set<T>().AddAsync(entity).AsTask();
        }

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">Entity's type.</typeparam>
        /// <param name="entity">Entity.</param>
        /// <returns></returns>
        public Task Update<T>(T entity)
             where T : class
        {
            ReadDbContext.Set<T>().Update(entity);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Persist changes into the database.
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChanges()
        {
            return ReadDbContext.SaveChangesAsync();
        }

        private IQueryable<T> ApplySpecification<T>(ISpecification<T> spec)
             where T : class
        {
            return SpecificationEvaluator<T>.GetQuery(ReadDbContext.Set<T>().AsQueryable(), spec);
        }
    }
}
