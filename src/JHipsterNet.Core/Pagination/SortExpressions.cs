using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JHipsterNet.Core.Pagination {
    public class SortExpressions<TEntity, TKey> {
        private readonly List<SortExpression<TEntity, TKey>> _expressionList;

        public SortExpressions()
        {
            _expressionList = new List<SortExpression<TEntity, TKey>>();
        }

        public void Add(Expression<Func<TEntity, TKey>> expression, bool isDescending = false)
        {
            _expressionList.Add(new SortExpression<TEntity, TKey> {
                Expression = expression,
                IsDescending = isDescending
            });
        }

        public static IQueryable<TEntity> ApplySorts(IQueryable<TEntity> query, SortExpressions<TEntity, TKey> sorts)
        {
            var isFirstSort = true;
            var validSortings = sorts.GetAll();
            IOrderedQueryable<TEntity> orderedQuery = null;

            foreach (var sort in validSortings) {
                orderedQuery = isFirstSort
                    ? ApplySorting(query, sort)
                    : ApplySorting(orderedQuery, sort);
                isFirstSort = false;
            }

            return orderedQuery ?? query;
        }

        private dynamic GetAll()
        {
            return _expressionList;
        }

        internal bool IsValid()
        {
            return _expressionList.Any();
        }

        private static IOrderedQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, SortExpression<TEntity, TKey> sortExpression)
        {
            return sortExpression.IsDescending
                ? query.OrderByDescending(sortExpression.Expression)
                : query.OrderBy(sortExpression.Expression);
        }

        private static IQueryable<TEntity> ApplySorting(IOrderedQueryable<TEntity> query, SortExpression<TEntity, TKey> sortExpression)
        {
            return sortExpression.IsDescending
                ? query.ThenByDescending(sortExpression.Expression)
                : query.ThenBy(sortExpression.Expression);
        }
    }
}
