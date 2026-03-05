using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MottuFlowApi.Models;
using MottuFlowApi.Services;
using MottuFlowApi.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuFlowApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ml")]
    [Tags("Machine Learning")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [AllowAnonymous]
    public class MlController : ControllerBase
    {
        private readonly MotoMlService _mlService;

        public MlController(MotoMlService mlService)
        {
            _mlService = mlService;
        }

        [HttpPost("predicao")]
        [SwaggerOperation(
            Summary = "Prediz necessidade de manutenção da moto",
            Description = "Usa um modelo de Machine Learning (ML.NET) para prever se uma moto precisa de manutenção com base na vibração, temperatura do motor, km rodados e idade do óleo.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Predição realizada com sucesso", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Entrada inválida", typeof(ApiResponse<string>))]
        public IActionResult Prever([FromBody] MotoInput input)
        {
            if (input == null)
                return BadRequest(ApiResponse<string>.Fail("Entrada inválida. O corpo da requisição não pode ser nulo."));

            if (input.Vibracao < 0 || input.TemperaturaMotor < 0 || input.KMRodados < 0 || input.IdadeOleoDias < 0)
                return BadRequest(ApiResponse<string>.Fail("Todos os valores numéricos devem ser maiores ou iguais a zero."));

            var motoParaPrever = new MotoData
            {
                Vibracao = input.Vibracao,
                TemperaturaMotor = input.TemperaturaMotor,
                KMRodados = input.KMRodados,
                IdadeOleoDias = input.IdadeOleoDias
            };

            var resultado = _mlService.Prever(motoParaPrever);

            var data = new
            {
                input.Vibracao,
                input.TemperaturaMotor,
                input.KMRodados,
                input.IdadeOleoDias,
                resultado.Predicao,
                resultado.Probabilidade,
                resultado.Score
            };

            return Ok(ApiResponse<object>.Ok(data, "Predição de manutenção realizada com sucesso."));
        }
    }

    public class MotoInput
    {
        public float Vibracao { get; set; }
        public float TemperaturaMotor { get; set; }
        public float KMRodados { get; set; }
        public float IdadeOleoDias { get; set; }
    }
}
