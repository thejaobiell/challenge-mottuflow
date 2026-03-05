using Microsoft.AspNetCore.Mvc;
using MottuFlowApi.Data;
using MottuFlowApi.DTOs;
using MottuFlowApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace MottuFlowApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [SwaggerTag("Autenticação")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly AppDbContext _dbContext;

        public AuthController(JwtService jwtService, AppDbContext dbContext)
        {
            _jwtService = jwtService;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Autentica um usuário e gera um token JWT")]
        [SwaggerResponse(StatusCodes.Status200OK, "Autenticação realizada com sucesso")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Credenciais inválidas")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação")]
        public IActionResult Login([FromBody] AuthLoginInputDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Usuário e senha são obrigatórios.");

            var user = _dbContext.Funcionarios.FirstOrDefault(f => f.Email == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Senha))
                return Unauthorized("Credenciais inválidas.");

            var token = _jwtService.GenerateToken(user.Email, user.Cargo);

            return Ok(new
            {
                token,
                role = user.Cargo,
                expiresIn = "2h"
            });
        }
    }
}