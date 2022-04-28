using System.Linq;
using System.Net.Http;
using JHipsterNet.Core.Pagination;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JHipsterNet.Web.Pagination.Swagger
{
    public class PageableModelFilter {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {
            var description = context.ApiDescription;
            if (description.HttpMethod != null && description.HttpMethod.ToLower() != HttpMethod.Get.ToString().ToLower()) {
                // We only want to do this for GET requests, if this is not a
                // GET request, leave this operation as is, do not modify
                return;
            }

            if (description.ParameterDescriptions.Any(elt => elt.Type == typeof(IPageable))) {
                operation?.RequestBody?.Content?.Clear();
            }

            if (operation == null) {
                return;
            }

            // We cleared all the auto generated parameters
            // So now we are going to add back the three parameters page, size and sort

            // This first parameter is the zero based page number (offset)
            operation.Parameters.Add(new OpenApiParameter {
                Name = "page",
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "number" }
            });

            // This parameter is the number of entities on each page
            operation.Parameters.Add(new OpenApiParameter {
                Name = "size",
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "number" }
            });

            // This parameter describes how the list should be sorted
            operation.Parameters.Add(new OpenApiParameter {
                Name = "sort",
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema() { Type = "string" }
            });
        }
    }
}
