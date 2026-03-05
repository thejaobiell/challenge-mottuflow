using Microsoft.AspNetCore.Mvc;
using MottuFlowApi.Utils;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace MottuFlowApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/health")]
    [Tags("Health Check")]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        private static readonly Stopwatch _uptime = Stopwatch.StartNew();

        /// <summary>
        /// Endpoint de verificação da saúde da API.
        /// </summary>
        [HttpGet("ping")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Verifica o status de funcionamento da API",
            Description = "Retorna informações sobre o estado atual da aplicação, tempo de execução, ambiente e timestamp UTC.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult Ping()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Desconhecido";

            // Simulação de checagem (você pode integrar health checks reais depois)
            bool apiSaudavel = true;

            var data = new
            {
                status = apiSaudavel ? "Healthy" : "Unhealthy",
                version = "1.0.0",
                uptime = $"{_uptime.Elapsed.Hours:D2}:{_uptime.Elapsed.Minutes:D2}:{_uptime.Elapsed.Seconds:D2}",
                environment,
                host = Environment.MachineName,
                timestampUtc = DateTime.UtcNow
            };

            if (!apiSaudavel)
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ApiResponse<object>.Fail("A API não está saudável no momento.", data));

            return Ok(ApiResponse<object>.Ok(data, "API rodando com sucesso 🚀"));
        }
    }
}
