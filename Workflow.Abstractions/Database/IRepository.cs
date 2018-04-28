using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Workflow.Abstractions.Database
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> ToListAsync();

        Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<TEntity> RemoveAsync(TEntity entity);

        Task<TEntity> InsertAsync(TEntity entity);

        Task InsertAsync(IEnumerable<TEntity> entities);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TEntity Update(TEntity entity);

        TEntity Remove(TEntity entity);

        TEntity Insert(TEntity entity);

        IQueryable<TEntity> Queryable();
    }
}
