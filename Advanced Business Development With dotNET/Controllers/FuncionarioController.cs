using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using MottuFlowApi.Data;
using MottuFlowApi.Models;
using MottuFlowApi.DTOs;
using MottuFlow.Hateoas;
using MottuFlowApi.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuFlowApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/funcionarios")]
    [Tags("Funcionários")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        // POST - Cria novo funcionário
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo funcionário", Description = "Cadastra um novo funcionário no sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Funcionário criado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "CPF ou Email já cadastrado.")]
        public async Task<IActionResult> CreateFuncionario([FromBody] FuncionarioInputDTO input)
        {
            if (input == null)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            if (await _context.Funcionarios.AnyAsync(f => f.CPF == input.Cpf))
                return Conflict(ApiResponse<string>.Fail("CPF já cadastrado."));

            if (await _context.Funcionarios.AnyAsync(f => f.Email == input.Email))
                return Conflict(ApiResponse<string>.Fail("Email já cadastrado."));

            var funcionario = new Funcionario
            {
                Nome = input.Nome,
                CPF = input.Cpf,
                Cargo = input.Cargo,
                Telefone = input.Telefone,
                Email = input.Email,
                Senha = HashSenha(input.Senha)
            };

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            var output = new FuncionarioOutputDTO
            {
                Id = funcionario.IdFuncionario,
                Nome = funcionario.Nome,
                Cpf = funcionario.CPF,
                Cargo = funcionario.Cargo,
                Telefone = funcionario.Telefone,
                Email = funcionario.Email,
                DataCadastro = DateTime.Now
            };

            return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.IdFuncionario }, 
                ApiResponse<FuncionarioOutputDTO>.Ok(output, "Funcionário criado com sucesso."));
        }

        // Hash seguro
        private string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha ?? string.Empty);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // GET - Lista funcionários
        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os funcionários", Description = "Retorna uma lista paginada de funcionários com filtros opcionais.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Funcionários listados com sucesso.")]
        public async Task<IActionResult> GetFuncionarios(string? nome = null, string? cargo = null, string? orderBy = "nome", int page = 1, int pageSize = 10)
        {
            var query = _context.Funcionarios.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(f => EF.Functions.Like(f.Nome.ToLower(), $"%{nome.ToLower()}%"));

            if (!string.IsNullOrEmpty(cargo))
                query = query.Where(f => EF.Functions.Like(f.Cargo.ToLower(), $"%{cargo.ToLower()}%"));

            query = orderBy?.ToLower() switch
            {
                "cargo" => query.OrderBy(f => f.Cargo),
                "email" => query.OrderBy(f => f.Email),
                _ => query.OrderBy(f => f.Nome)
            };

            var totalItems = await query.CountAsync();

            var funcionarios = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new FuncionarioOutputDTO
                {
                    Id = f.IdFuncionario,
                    Nome = f.Nome!,
                    Cpf = f.CPF!,
                    Cargo = f.Cargo!,
                    Telefone = f.Telefone!,
                    Email = f.Email!,
                    DataCadastro = DateTime.Now.AddDays(-f.IdFuncionario)
                })
                .ToListAsync();

            var meta = new
            {
                totalItems,
                page,
                pageSize,
                totalPages = Math.Ceiling((double)totalItems / pageSize)
            };

            return Ok(ApiResponse<object>.Ok(new { meta, funcionarios }, "Funcionários listados com sucesso."));
        }

        // GET - Por ID
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um funcionário específico", Description = "Retorna os dados detalhados de um funcionário.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Funcionário encontrado.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Funcionário não encontrado.")]
        public async Task<IActionResult> GetFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios
                .Where(f => f.IdFuncionario == id)
                .Select(f => new FuncionarioOutputDTO
                {
                    Id = f.IdFuncionario,
                    Nome = f.Nome!,
                    Cpf = f.CPF!,
                    Cargo = f.Cargo!,
                    Telefone = f.Telefone!,
                    Email = f.Email!,
                    DataCadastro = DateTime.Now.AddDays(-f.IdFuncionario)
                })
                .FirstOrDefaultAsync();

            if (funcionario == null)
                return NotFound(ApiResponse<string>.Fail("Funcionário não encontrado."));

            return Ok(ApiResponse<FuncionarioOutputDTO>.Ok(funcionario, "Funcionário encontrado com sucesso."));
        }

        // PUT - Atualiza
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um funcionário existente", Description = "Permite atualizar dados de um funcionário pelo ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Funcionário atualizado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Funcionário não encontrado.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro na requisição.")]
        public async Task<IActionResult> UpdateFuncionario(int id, [FromBody] FuncionarioInputDTO input)
        {
            if (input == null)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
                return NotFound(ApiResponse<string>.Fail("Funcionário não encontrado."));

            funcionario.Nome = input.Nome;
            funcionario.Cargo = input.Cargo;
            funcionario.Telefone = input.Telefone;
            funcionario.Email = input.Email;

            if (!string.IsNullOrEmpty(input.Senha))
                funcionario.Senha = HashSenha(input.Senha);

            _context.Entry(funcionario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<string>.Ok("Funcionário atualizado com sucesso."));
        }

        // DELETE - Remove
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove um funcionário", Description = "Exclui o funcionário do sistema.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Funcionário removido com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Funcionário não encontrado.")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
                return NotFound(ApiResponse<string>.Fail("Funcionário não encontrado."));

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}