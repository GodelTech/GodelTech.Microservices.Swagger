using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GodelTech.Microservices.Swagger.Swagger
{
    public interface IAuthorizeCheckOperationFilter
    {
        void Apply(OpenApiOperation operation, OperationFilterContext context);
    }
}
