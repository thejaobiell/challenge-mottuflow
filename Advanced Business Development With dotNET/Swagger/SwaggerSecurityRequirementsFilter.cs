using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MottuFlowApi.Swagger
{
    public class SwaggerSecurityRequirementsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Verifica se o endpoint tem [AllowAnonymous]
            var hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true)
                .Any(attr => attr is AllowAnonymousAttribute);

            var controllerHasAllowAnonymous = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .Any(attr => attr is AllowAnonymousAttribute) ?? false;

            // Se NÃO tem [AllowAnonymous], adiciona o Bearer
            if (!hasAllowAnonymous && !controllerHasAllowAnonymous)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                };
            }
        }
    }
}