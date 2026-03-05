using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("api/v{version:apiVersion}/motos")]
    [Tags("Motos")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize] // exige token para operações sensíveis
    public class MotoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MotoController(AppDbContext context) => _context = context;

        // HATEOAS links
        private void AddHateoasLinks(MotoResource resource, int id)
        {
            resource.AddLink(new Link { Href = Url.Link(nameof(GetMoto), new { id })!, Rel = "self", Method = "GET" });
            resource.AddLink(new Link { Href = Url.Link(nameof(UpdateMoto), new { id })!, Rel = "update", Method = "PUT" });
            resource.AddLink(new Link { Href = Url.Link(nameof(DeleteMoto), new { id })!, Rel = "delete", Method = "DELETE" });
        }

        // GET - Listar todas as motos
        [HttpGet(Name = "GetMotos")]
        [SwaggerOperation(Summary = "Lista todas as motos", Description = "Retorna uma lista paginada de motos cadastradas.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de motos retornada com sucesso")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]
        public async Task<IActionResult> GetMotos(int page = 1, int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 1);

            var totalItems = await _context.Motos.CountAsync();

            var motos = await _context.Motos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MotoResource
                {
                    Id = m.IdMoto,
                    Placa = m.Placa!,
                    Modelo = m.Modelo!,
                    Fabricante = m.Fabricante!,
                    Ano = m.Ano,
                    IdPatio = m.IdPatio,
                    LocalizacaoAtual = m.LocalizacaoAtual!
                })
                .ToListAsync();

            if (!motos.Any())
                return Ok(ApiResponse<object>.Ok(new { totalItems = 0, data = new List<MotoResource>() }, "Nenhuma moto cadastrada."));

            motos.ForEach(m => AddHateoasLinks(m, m.Id));

            var meta = new
            {
                totalItems,
                page,
                pageSize,
                totalPages = Math.Ceiling((double)totalItems / pageSize)
            };

            return Ok(ApiResponse<object>.Ok(new { meta, data = motos }, "Motos listadas com sucesso."));
        }

        // GET - Buscar moto por ID
        [HttpGet("{id}", Name = "GetMoto")]
        [SwaggerOperation(Summary = "Obtém uma moto específica", Description = "Retorna os detalhes de uma moto pelo seu ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Moto encontrada com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        public async Task<IActionResult> GetMoto(int id)
        {
            var moto = await _context.Motos
                .Where(m => m.IdMoto == id)
                .Select(m => new MotoResource
                {
                    Id = m.IdMoto,
                    Placa = m.Placa!,
                    Modelo = m.Modelo!,
                    Fabricante = m.Fabricante!,
                    Ano = m.Ano,
                    IdPatio = m.IdPatio,
                    LocalizacaoAtual = m.LocalizacaoAtual!
                })
                .FirstOrDefaultAsync();

            if (moto == null)
                return NotFound(ApiResponse<string>.Fail("Moto não encontrada."));

            AddHateoasLinks(moto, moto.Id);
            return Ok(ApiResponse<MotoResource>.Ok(moto, "Moto encontrada com sucesso."));
        }

        // POST - Criar nova moto
        [HttpPost(Name = "CreateMoto")]
        [SwaggerOperation(Summary = "Cria uma nova moto", Description = "Adiciona uma nova moto no sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Moto criada com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro na requisição ou dados inválidos")]
        public async Task<IActionResult> CreateMoto([FromBody] MotoInputDTO input)
        {
            if (input == null)
                return BadRequest(ApiResponse<string>.Fail("Input não pode ser nulo."));

            var moto = new Moto
            {
                Placa = input.Placa,
                Modelo = input.Modelo,
                Fabricante = input.Fabricante,
                Ano = input.Ano,
                IdPatio = input.IdPatio,
                LocalizacaoAtual = input.LocalizacaoAtual
            };

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            var resource = new MotoResource
            {
                Id = moto.IdMoto,
                Placa = moto.Placa,
                Modelo = moto.Modelo,
                Fabricante = moto.Fabricante,
                Ano = moto.Ano,
                IdPatio = moto.IdPatio,
                LocalizacaoAtual = moto.LocalizacaoAtual
            };

            AddHateoasLinks(resource, moto.IdMoto);

            return CreatedAtAction(nameof(GetMoto), new { id = moto.IdMoto },
                ApiResponse<MotoResource>.Ok(resource, "Moto criada com sucesso."));
        }

        // PUT - Atualizar moto existente
        [HttpPut("{id}", Name = "UpdateMoto")]
        [SwaggerOperation(Summary = "Atualiza os dados de uma moto", Description = "Modifica informações de uma moto existente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Moto atualizada com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação ou dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        public async Task<IActionResult> UpdateMoto(int id, [FromBody] MotoInputDTO input)
        {
            if (input == null)
                return BadRequest(ApiResponse<string>.Fail("Input não pode ser nulo."));

            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound(ApiResponse<string>.Fail("Moto não encontrada."));

            moto.Placa = input.Placa;
            moto.Modelo = input.Modelo;
            moto.Fabricante = input.Fabricante;
            moto.Ano = input.Ano;
            moto.IdPatio = input.IdPatio;
            moto.LocalizacaoAtual = input.LocalizacaoAtual;

            _context.Entry(moto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var updated = new MotoResource
            {
                Id = moto.IdMoto,
                Placa = moto.Placa,
                Modelo = moto.Modelo,
                Fabricante = moto.Fabricante,
                Ano = moto.Ano,
                IdPatio = moto.IdPatio,
                LocalizacaoAtual = moto.LocalizacaoAtual
            };

            AddHateoasLinks(updated, moto.IdMoto);

            return Ok(ApiResponse<MotoResource>.Ok(updated, "Moto atualizada com sucesso."));
        }

        // DELETE - Remover moto
        [HttpDelete("{id}", Name = "DeleteMoto")]
        [SwaggerOperation(Summary = "Remove uma moto", Description = "Exclui uma moto cadastrada do sistema.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Moto removida com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound(ApiResponse<string>.Fail("Moto não encontrada."));

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
