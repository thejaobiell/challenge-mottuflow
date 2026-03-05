using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuFlow.DTOs;
using MottuFlowApi.Models;
using MottuFlowApi.Data;
using MottuFlowApi.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuFlowApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/registro-status")]
    [Tags("Registros de Status")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize] // exige JWT para operações de escrita
    public class RegistroStatusController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegistroStatusController(AppDbContext context)
        {
            _context = context;
        }

        //  GET - Lista com paginação
        [HttpGet(Name = "GetRegistroStatus")]
        [SwaggerOperation(Summary = "Lista todos os registros de status", Description = "Retorna uma lista paginada de registros de status das motos.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Registros de status retornados com sucesso")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]
        public async Task<IActionResult> GetRegistroStatus(int page = 1, int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 1);

            var totalItems = await _context.RegistroStatuses.CountAsync();

            var registros = await _context.RegistroStatuses
                .OrderByDescending(r => r.DataStatus)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new StatusDTO
                {
                    TipoStatus = r.TipoStatus,
                    Descricao = r.Descricao ?? string.Empty,
                    DataStatus = r.DataStatus,
                    IdMoto = r.IdMoto,
                    IdFuncionario = r.IdFuncionario
                })
                .ToListAsync();

            if (!registros.Any())
                return Ok(ApiResponse<object>.Ok(new { totalItems = 0, data = new List<StatusDTO>() }, "Nenhum registro de status encontrado."));

            var meta = new
            {
                totalItems,
                page,
                pageSize,
                totalPages = Math.Ceiling((double)totalItems / pageSize)
            };

            return Ok(ApiResponse<object>.Ok(new { meta, data = registros }, "Registros de status listados com sucesso."));
        }

        //  GET - Por ID
        [HttpGet("{id}", Name = "GetRegistroStatusById")]
        [SwaggerOperation(Summary = "Obtém um registro de status específico", Description = "Retorna os detalhes de um registro de status pelo ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Registro de status encontrado com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Registro de status não encontrado")]
        public async Task<IActionResult> GetRegistroStatusById(int id)
        {
            var r = await _context.RegistroStatuses.FindAsync(id);
            if (r == null)
                return NotFound(ApiResponse<string>.Fail("Registro de status não encontrado."));

            var dto = new StatusDTO
            {
                TipoStatus = r.TipoStatus,
                Descricao = r.Descricao ?? string.Empty,
                DataStatus = r.DataStatus,
                IdMoto = r.IdMoto,
                IdFuncionario = r.IdFuncionario
            };

            return Ok(ApiResponse<StatusDTO>.Ok(dto, "Registro de status encontrado com sucesso."));
        }

        //  POST - Criar novo registro
        [HttpPost(Name = "CreateRegistroStatus")]
        [SwaggerOperation(Summary = "Cria um novo registro de status", Description = "Registra um novo status de moto no sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Registro de status criado com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação nos dados")]
        public async Task<IActionResult> CreateRegistroStatus([FromBody] StatusDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var r = new RegistroStatus
            {
                TipoStatus = dto.TipoStatus,
                Descricao = dto.Descricao,
                DataStatus = dto.DataStatus == default ? DateTime.UtcNow : dto.DataStatus,
                IdMoto = dto.IdMoto,
                IdFuncionario = dto.IdFuncionario
            };

            _context.RegistroStatuses.Add(r);
            await _context.SaveChangesAsync();

            var result = new StatusDTO
            {
                TipoStatus = r.TipoStatus,
                Descricao = r.Descricao,
                DataStatus = r.DataStatus,
                IdMoto = r.IdMoto,
                IdFuncionario = r.IdFuncionario
            };

            return CreatedAtAction(nameof(GetRegistroStatusById), new { id = r.IdStatus },
                ApiResponse<StatusDTO>.Ok(result, "Registro de status criado com sucesso."));
        }

// PUT - Atualizar registro existente
        [HttpPut("{id}", Name = "UpdateRegistroStatus")]
        [SwaggerOperation(Summary = "Atualiza um registro de status existente", Description = "Permite alterar os dados de um registro de status.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Registro de status atualizado com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Registro de status não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação nos dados")]
        public async Task<IActionResult> UpdateRegistroStatus(int id, [FromBody] StatusDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var r = await _context.RegistroStatuses.FindAsync(id);
            if (r == null)
                return NotFound(ApiResponse<string>.Fail("Registro de status não encontrado."));

            r.TipoStatus = dto.TipoStatus;
            r.Descricao = dto.Descricao;
            r.DataStatus = dto.DataStatus;
            r.IdMoto = dto.IdMoto;
            r.IdFuncionario = dto.IdFuncionario;

            _context.Entry(r).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var updated = new StatusDTO
            {
                TipoStatus = r.TipoStatus,
                Descricao = r.Descricao,
                DataStatus = r.DataStatus,
                IdMoto = r.IdMoto,
                IdFuncionario = r.IdFuncionario
            };

            return Ok(ApiResponse<StatusDTO>.Ok(updated, "Registro de status atualizado com sucesso."));
        }

        //  DELETE - Remover registro
        [HttpDelete("{id}", Name = "DeleteRegistroStatus")]
        [SwaggerOperation(Summary = "Remove um registro de status", Description = "Exclui um registro de status existente pelo ID.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Registro de status removido com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Registro de status não encontrado")]
        public async Task<IActionResult> DeleteRegistroStatus(int id)
        {
            var r = await _context.RegistroStatuses.FindAsync(id);
            if (r == null)
                return NotFound(ApiResponse<string>.Fail("Registro de status não encontrado."));

            _context.RegistroStatuses.Remove(r);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
