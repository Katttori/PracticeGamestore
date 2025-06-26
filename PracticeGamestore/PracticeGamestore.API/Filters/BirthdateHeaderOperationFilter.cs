using Microsoft.OpenApi.Models;
using PracticeGamestore.Business.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PracticeGamestore.Filters;

public class BirthdateHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasBirthdateFilter =
            context.MethodInfo.GetCustomAttributes(typeof(BirthdateRestrictionFilter), false).Any();
        
        if(!hasBirthdateFilter)return;

        operation.Parameters ??= new List<OpenApiParameter>();
        
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = HeaderNames.Birthdate,
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema 
            { 
                Type = "string", 
                Format = "date",
                Example = new Microsoft.OpenApi.Any.OpenApiString("2000-01-15")
            },
        });
    }
}