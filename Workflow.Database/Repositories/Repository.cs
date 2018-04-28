using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Workflow.Abstractions.Database;
using Workflow.Abstractions.Models;
using Workflow.Core.Interfaces;

namespace Workflow.Database
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public IBag _bag { get; set; }

        private readonly WorkflowContext _dbContext;


        public Repository(WorkflowContext dbContext, IBag bag)
        {
            _dbContext = dbContext;

            _bag = bag;
        }

        public async Task<List<TEntity>> ToListAsync()
        {
            return await GetFilteredQueryable().ToListAsync();
        }

        public async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetFilteredQueryable().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetFilteredQueryable().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var result = _dbContext.Set<TEntity>().Update(entity);

            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<TEntity> RemoveAsync(TEntity entity)
        {
            var result = _dbContext.Set<TEntity>().Remove(entity);

            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var result = _dbContext.Set<TEntity>().Add(entity);

            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            var result = _dbContext.Set<TEntity>().AddRangeAsync(entities);

            await _dbContext.SaveChangesAsync();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetFilteredQueryable().FirstOrDefault(predicate);
        }

        public TEntity Update(TEntity entity)
        {
            var result = _dbContext.Set<TEntity>().Update(entity);

            _dbContext.SaveChanges();

            return result.Entity;
        }

        public TEntity Remove(TEntity entity)
        {
            var result = _dbContext.Set<TEntity>().Remove(entity);

            _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public TEntity Insert(TEntity entity)
        {
            var result = _dbContext.Set<TEntity>().Add(entity);

            _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public IQueryable<TEntity> Queryable()
        {
            return GetFilteredQueryable();
        }

        private IQueryable<TEntity> GetFilteredQueryable()
        {
            if (!typeof(IProjectRelated).IsAssignableFrom(typeof(TEntity)) || _bag.CurrentProjectId == Guid.Empty)
            {
                return _dbContext.Set<TEntity>();
            }

            return _dbContext.Set<TEntity>().Where(e => EF.Property<Guid>(e, "ProjectId") == _bag.CurrentProjectId);
        }
    }
}
