using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using JHipsterNet.Web.Pagination.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JHipsterNet.Web.Pagination.Binders {
    public class PageableBinder : IModelBinder {
        private readonly PageableBinderConfig _binderConfig = new PageableBinderConfig();

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //TODO Assert target method (only one pageable)
            //TODO defensive programing against PageableBinderConfig values

            if (bindingContext == null) {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var queryString = bindingContext.HttpContext.Request.QueryString;
            var pageable = ResolvePageableArgumentFromQueryString(queryString);
            bindingContext.Result = ModelBindingResult.Success(pageable);
            return Task.CompletedTask;
        }

        private IPageable ResolvePageableArgumentFromQueryString(QueryString queryString)
        {
            var pageNumberString = queryString.GetParameter(_binderConfig.PageParameterName);
            var pageSizeString = queryString.GetParameter(_binderConfig.SizeParameterName);

            var pageNumber = ParseIntOrDefault(pageNumberString, _binderConfig.FallbackPageable.PageNumber);
            var pageSize = ParseIntOrDefault(pageSizeString, _binderConfig.FallbackPageable.PageSize,
                _binderConfig.MaxPageSize);

            var sort = ResolveSortArgument(queryString);
            if (sort != null) {
                return Pageable.Of(pageNumber, pageSize, sort);
            }

            return Pageable.Of(pageNumber, pageSize);
        }

        private static int ParseIntOrDefault(string parameter, int defaultValue, int upper = int.MaxValue)
        {
            if (!int.TryParse(parameter, out var value)) {
                value = defaultValue;
            }

            value = value < 0 ? 0 : value;
            value = value > upper ? upper : value;

            return value;
        }

        private Sort ResolveSortArgument(QueryString queryString)
        {
            var sortParts = queryString.GetParameterValues(_binderConfig.SortParameterName);
            var orders = new List<Order>();
            foreach (var sortPart in sortParts) {
                var sortSpt = sortPart.Split(_binderConfig.SortDelimiter);
                var property = sortSpt?[0];
                var direction = sortSpt != null && sortSpt.Length == 2 && sortSpt[1] == "desc" ? Direction.Desc : Sort.DefaultDirection;
                if (!string.IsNullOrEmpty(property)) {
                    orders.Add(new Order(direction, property));
                }
            }
            return new Sort(orders);
        }
    }
}
