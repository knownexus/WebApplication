using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerDefault401Filter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!operation.Responses.ContainsKey("401"))
        {
            operation.Responses.Add("401", new OpenApiResponse
            {
                Description = "Unauthorized (returns UnauthorizedResponseModel)"
            });
        }
    }
}
