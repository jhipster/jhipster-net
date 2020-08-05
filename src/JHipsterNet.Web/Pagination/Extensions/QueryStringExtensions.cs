using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace JHipsterNet.Web.Pagination.Extensions {
    public static class QueryStringExtensions {
        public static string GetParameter(this QueryString query, string name)
        {
            if (query == null || string.IsNullOrEmpty(name))
                return null;
            var parameters = QueryHelpers.ParseQuery(query.ToString());
            return parameters.ContainsKey(name) ? parameters[name][0] : null;
        }

        public static string[] GetParameterValues(this QueryString query, string name)
        {
            if (query == null || string.IsNullOrEmpty(name))
                return new string[0];
            var parameters = QueryHelpers.ParseQuery(query.ToString());
            return parameters.ContainsKey(name) ? parameters[name].ToArray() : new string[0];
        }
    }
}
