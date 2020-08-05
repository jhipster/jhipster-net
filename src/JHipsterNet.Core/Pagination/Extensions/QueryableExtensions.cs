using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace JHipsterNet.Core.Pagination.Extensions {
    public static class QueryableExtensions {

        public static async Task<IPage<TDto>> UsePageableAsDtoAsync<TEntity, TDto>(this IQueryable<TEntity> query, IPageable pageable, IMapper mapper)
            where TEntity : class
            where TDto : class
        {
            var entitiesQuery = query.ApplySort(pageable.Sort).Skip(pageable.Offset).Take(Math.Min(pageable.PageSize, PageableBinderConfig.DefaultMaxPageSize));
            var entitiesList = await entitiesQuery
                                        .AsNoTracking()//speed up because we will not make changes to the entities
                                        .Select(entity => mapper.Map<TDto>(entity))
                                        .ToListAsync();

            return new Page<TDto>(entitiesList, pageable, query.Count());
        }

        public static async Task<IPage<TEntity>> UsePageableAsync<TEntity>(this IQueryable<TEntity> query, IPageable pageable)
            where TEntity : class
        {
            var entitiesQuery = query.ApplySort(pageable.Sort).Skip(pageable.Offset).Take(Math.Min(pageable.PageSize, PageableBinderConfig.DefaultMaxPageSize));
            var entitiesList = await entitiesQuery
                                        .AsNoTracking()//speed up because we will not make changes to the entities
                                        .ToListAsync();
            return new Page<TEntity>(entitiesList, pageable, query.Count());
        }

        public static IPage<TEntity> UsePageable<TEntity>(this IQueryable<TEntity> query, IPageable pageable)
            where TEntity : class
        {
            var entities = query.ApplySort(pageable.Sort).Skip(pageable.Offset).Take(Math.Min(pageable.PageSize, PageableBinderConfig.DefaultMaxPageSize));
            return new Page<TEntity>(entities.ToList(), pageable, query.Count());
        }

        private static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> query, Sort sort)
        {
            if (!query.Any() || sort == null || sort.IsUnsorted()) {
                return query;
            }

            var sortExpressions = new SortExpressions<TEntity, object>();
            var propertyInfos = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orders = sort.Orders;
            foreach (var order in orders) {
                if (order == null || order.Property == null) {
                    continue;
                }

                var isDescending = order.Direction.IsDescending();
                var propertyInfo = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(order.Property, StringComparison.InvariantCultureIgnoreCase));
                if (propertyInfo == null) {
                    continue;
                }

                var expressionFunc = GetExpression<TEntity, object>(propertyInfo);
                sortExpressions.Add(expressionFunc, isDescending);
            }
            return SortExpressions<TEntity, object>.ApplySorts(query, sortExpressions);
        }

        private static Expression<Func<TEntity, TKey>> GetExpression<TEntity, TKey>(PropertyInfo propertyInfo)
        {
            var ep = Expression.Parameter(typeof(TEntity), "x");
            var em = Expression.Property(ep, propertyInfo);
            return Expression.Lambda<Func<TEntity, TKey>>(Expression.Convert(em, typeof(object)), ep);
        }
    }
}
