﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ReadRepositories
{
    public interface IGenericRepository
    {
        Task Add<T>(T entity) where T : class;

        Task Update<T>(T entity) where T : class;

        IQueryable<T> Query<T>() where T : class;

        Task Delete<T>(T entity) where T : class;

        Task<int> SaveChanges();
    }
}
