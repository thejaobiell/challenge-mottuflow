using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Data;
using MottuFlowApi.Models;
using MottuFlowApi.DTOs;
using MottuFlow.Hateoas;
using MottuFlowApi.Utils;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace MottuFlowApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/patios")]
    [Tags("Pátios")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize] // Protege os endpoints sensíveis
    public class PatioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatioController(AppDbContext context)
        {
            _context = context;
        }

        // Adiciona links HATEOAS
        private void AddHateoasLinks(PatioResource resource, int id)
        {
            resource.AddLink(new Link { Href = Url.Link(nameof(GetPatio), new { id })!, Rel = "self", Method = "GET" });
            resource.AddLink(new Link { Href = Url.Link(nameof(UpdatePatio), new { id })!, Rel = "update", Method = "PUT" });
            resource.AddLink(new Link { Href = Url.Link(nameof(DeletePatio), new { id })!, Rel = "delete", Method = "DELETE" });
        }

        // GET - Todos os pátios
        [HttpGet(Name = "GetPatios")]
        [SwaggerOperation(Summary = "Lista todos os pátios", Description = "Retorna uma lista paginada de pátios cadastrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista retornada com sucesso")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]
        public async Task<IActionResult> GetPatios(int page = 1, int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 1);

            var totalItems = await _context.Patios.CountAsync();

            var patios = await _context.Patios
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PatioResource
                {
                    Id = p.IdPatio,
                    Nome = p.Nome!,
                    Endereco = p.Endereco!,
                    CapacidadeMaxima = p.CapacidadeMaxima
                })
                .ToListAsync();

            if (!patios.Any())
                return Ok(ApiResponse<object>.Ok(new { totalItems = 0, data = new List<PatioResource>() }, "Nenhum pátio encontrado."));

            patios.ForEach(p => AddHateoasLinks(p, p.Id));

            var meta = new
            {
                totalItems,
                page,
                pageSize,
                totalPages = Math.Ceiling((double)totalItems / pageSize)
            };

            return Ok(ApiResponse<object>.Ok(new { meta, data = patios }, "Pátios listados com sucesso."));
        }

        // GET - Por ID
        [HttpGet("{id}", Name = "GetPatio")]
        [SwaggerOperation(Summary = "Obtém um pátio específico", Description = "Retorna os detalhes de um pátio pelo seu ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Pátio encontrado com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pátio não encontrado")]
        public async Task<IActionResult> GetPatio(int id)
        {
            var patio = await _context.Patios
                .Where(p => p.IdPatio == id)
                .Select(p => new PatioResource
                {
                    Id = p.IdPatio,
                    Nome = p.Nome!,
                    Endereco = p.Endereco!,
                    CapacidadeMaxima = p.CapacidadeMaxima
                })
                .FirstOrDefaultAsync();

            if (patio == null)
                return NotFound(ApiResponse<string>.Fail("Pátio não encontrado."));

            AddHateoasLinks(patio, patio.Id);
            return Ok(ApiResponse<PatioResource>.Ok(patio, "Pátio encontrado com sucesso."));
        }

        // POST - Criar novo pátio (autenticado)
        [HttpPost(Name = "CreatePatio")]
        [SwaggerOperation(Summary = "Cria um novo pátio", Description = "Adiciona um novo pátio no sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Pátio criado com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação nos dados")]
        public async Task<IActionResult> CreatePatio([FromBody][Required] PatioInputDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var patio = new Patio
            {
                Nome = input.Nome,
                Endereco = input.Endereco,
                CapacidadeMaxima = input.CapacidadeMaxima
            };

            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            var resource = new PatioResource
            {
                Id = patio.IdPatio,
                Nome = patio.Nome,
                Endereco = patio.Endereco,
                CapacidadeMaxima = patio.CapacidadeMaxima
            };

            AddHateoasLinks(resource, patio.IdPatio);

            return CreatedAtAction(nameof(GetPatio), new { id = patio.IdPatio },
                ApiResponse<PatioResource>.Ok(resource, "Pátio criado com sucesso."));
        }

        // PUT - Atualizar pátio
        [HttpPut("{id}", Name = "UpdatePatio")]
        [SwaggerOperation(Summary = "Atualiza um pátio existente", Description = "Permite alterar dados de um pátio cadastrado.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Pátio atualizado com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pátio não encontrado")]
        public async Task<IActionResult> UpdatePatio(int id, [FromBody][Required] PatioInputDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound(ApiResponse<string>.Fail("Pátio não encontrado."));

            patio.Nome = input.Nome;
            patio.Endereco = input.Endereco;
            patio.CapacidadeMaxima = input.CapacidadeMaxima;

            _context.Entry(patio).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var updated = new PatioResource
            {
                Id = patio.IdPatio,
                Nome = patio.Nome,
                Endereco = patio.Endereco,
                CapacidadeMaxima = patio.CapacidadeMaxima
            };

            AddHateoasLinks(updated, patio.IdPatio);

            return Ok(ApiResponse<PatioResource>.Ok(updated, "Pátio atualizado com sucesso."));
        }

                // DELETE - Remover pátio
        [HttpDelete("{id}", Name = "DeletePatio")]
        [SwaggerOperation(Summary = "Remove um pátio", Description = "Exclui um pátio cadastrado no sistema.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Pátio removido com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pátio não encontrado")]
        public async Task<IActionResult> DeletePatio(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound(ApiResponse<object>.Fail("Pátio não encontrado."));

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

