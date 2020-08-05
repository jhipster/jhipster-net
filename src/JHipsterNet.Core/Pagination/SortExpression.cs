using System;
using System.Linq.Expressions;

namespace JHipsterNet.Core.Pagination {
    public class SortExpression<TEntity, TKey> {
        public Expression<Func<TEntity, TKey>> Expression { get; set; }
        public bool IsDescending { get; set; }
    }
}
