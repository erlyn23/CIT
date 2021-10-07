﻿using CIT.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CIT.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CIT.DataAccess.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CentroInversiontesTecnocorpDbContext _dbContext;

        public GenericRepository(CentroInversiontesTecnocorpDbContext dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(T[] entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return entities.ToList();
        }

        public void Delete(T entity) =>
            _dbContext.Set<T>().Remove(entity);

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression) =>
            await _dbContext.Set<T>().Where(expression).FirstOrDefaultAsync();

        public async Task<List<T>> GetAllAsync() =>
            await _dbContext.Set<T>().ToListAsync();

        public async Task<List<T>> GetAllWithFilterAsync(Expression<Func<T, bool>> expression) =>
            await _dbContext.Set<T>().Where(expression).ToListAsync();

        public T Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
