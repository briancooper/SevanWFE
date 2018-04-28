using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Workflow.Application.Controllers.Dto;

namespace Workflow.Application.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition,
            Func<T, bool> predicate)
        {
            if (!condition)
            {
                return enumerable;
            }

            return enumerable.Where(predicate);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, bool condition,
            Expression<Func<T, bool>> expression)
        {
            if (!condition)
            {
                return queryable;
            }

            return queryable.Where(expression);
        }

        public static async Task<PagedResultDto<TDto>> ToPagedResult<TEntity, TDto>(this IQueryable<TEntity> queryable, IFullResultInput input, Expression<Func<TEntity, bool>> predicateFilter) where TDto : class
        {
            queryable = queryable.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), predicateFilter);

            var count = await queryable.CountAsync();

            //var result = await queryable.OrderBy(input.Sorting).Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

            var result = await queryable.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

            return new PagedResultDto<TDto>(count, Mapper.Map<List<TDto>>(result));
        }

        public static async Task<PagedResultDto<TDto>> ToPagedResult<TEntity, TDto>(this IQueryable<TEntity> queryable, IPagedResultInput input, Expression<Func<TEntity, bool>> predicateFilter) where TDto : class
        {
            var count = await queryable.CountAsync();

            var result = await queryable.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

            return new PagedResultDto<TDto>(count, Mapper.Map<List<TDto>>(result));
        }
    }
}
