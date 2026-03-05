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
    [Route("api/v{version:apiVersion}/arucotags")]
    [Tags("ArucoTags")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize] // exige JWT para escrita
    public class ArucoTagController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ArucoTagController(AppDbContext context) => _context = context;

        // HATEOAS Links
        private void AddHateoasLinks(ArucoTagResource resource, int id)
        {
            resource.AddLink(new Link { Href = Url.Link(nameof(GetArucoTag), new { id })!, Rel = "self", Method = "GET" });
            resource.AddLink(new Link { Href = Url.Link(nameof(UpdateArucoTag), new { id })!, Rel = "update", Method = "PUT" });
            resource.AddLink(new Link { Href = Url.Link(nameof(DeleteArucoTag), new { id })!, Rel = "delete", Method = "DELETE" });
        }

        // GET - Lista todas as ArucoTags 
        [HttpGet(Name = "GetArucoTags")]
        [SwaggerOperation(Summary = "Lista todas as ArucoTags", Description = "Retorna uma lista de ArucoTags cadastradas no sistema.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista retornada com sucesso")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]
        public async Task<IActionResult> GetArucoTags()
        {
            var tags = await _context.ArucoTags
                .Select(t => new ArucoTagResource
                {
                    Id = t.IdTag,
                    Codigo = t.Codigo!,
                    Status = t.Status!,
                    IdMoto = t.IdMoto
                })
                .ToListAsync();

            if (!tags.Any())
                return Ok(ApiResponse<object>.Ok(new { totalItems = 0, data = new List<ArucoTagResource>() }, "Nenhuma ArucoTag cadastrada."));

            tags.ForEach(t => AddHateoasLinks(t, t.Id));

            return Ok(ApiResponse<IEnumerable<ArucoTagResource>>.Ok(tags, "ArucoTags listadas com sucesso."));
        }

        // GET - Retorna uma ArucoTag por ID 
        [HttpGet("{id}", Name = "GetArucoTag")]
        [SwaggerOperation(Summary = "Obtém uma ArucoTag específica", Description = "Retorna os dados de uma ArucoTag pelo ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "ArucoTag encontrada com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "ArucoTag não encontrada")]
        public async Task<IActionResult> GetArucoTag(int id)
        {
            var tag = await _context.ArucoTags
                .Where(t => t.IdTag == id)
                .Select(t => new ArucoTagResource
                {
                    Id = t.IdTag,
                    Codigo = t.Codigo!,
                    Status = t.Status!,
                    IdMoto = t.IdMoto
                })
                .FirstOrDefaultAsync();

            if (tag == null)
                return NotFound(ApiResponse<string>.Fail("ArucoTag não encontrada."));

            AddHateoasLinks(tag, tag.Id);
            return Ok(ApiResponse<ArucoTagResource>.Ok(tag, "ArucoTag encontrada com sucesso."));
        }

        // POST - Cria uma nova ArucoTag
        [HttpPost(Name = "CreateArucoTag")]
        [SwaggerOperation(Summary = "Cria uma nova ArucoTag", Description = "Registra uma nova ArucoTag associada a uma moto.")]
        [SwaggerResponse(StatusCodes.Status201Created, "ArucoTag criada com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação nos dados")]
        public async Task<IActionResult> CreateArucoTag([FromBody] ArucoTagInputDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var tag = new ArucoTag
            {
                Codigo = input.Codigo,
                Status = input.Status,
                IdMoto = input.IdMoto
            };

            _context.ArucoTags.Add(tag);
            await _context.SaveChangesAsync();

            var resource = new ArucoTagResource
            {
                Id = tag.IdTag,
                Codigo = tag.Codigo,
                Status = tag.Status,
                IdMoto = tag.IdMoto
            };

            AddHateoasLinks(resource, tag.IdTag);

            return CreatedAtAction(nameof(GetArucoTag), new { id = tag.IdTag },
                ApiResponse<ArucoTagResource>.Ok(resource, "ArucoTag criada com sucesso."));
        }

        // PUT - Atualiza uma ArucoTag existente
        [HttpPut("{id}", Name = "UpdateArucoTag")]
        [SwaggerOperation(Summary = "Atualiza uma ArucoTag existente", Description = "Permite atualizar os dados de uma ArucoTag cadastrada.")]
        [SwaggerResponse(StatusCodes.Status200OK, "ArucoTag atualizada com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "ArucoTag não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação nos dados")]
        public async Task<IActionResult> UpdateArucoTag(int id, [FromBody] ArucoTagInputDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var tag = await _context.ArucoTags.FindAsync(id);
            if (tag == null)
                return NotFound(ApiResponse<string>.Fail("ArucoTag não encontrada."));

            tag.Codigo = input.Codigo;
            tag.Status = input.Status;
            tag.IdMoto = input.IdMoto;

            _context.Entry(tag).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var updated = new ArucoTagResource
            {
                Id = tag.IdTag,
                Codigo = tag.Codigo,
                Status = tag.Status,
                IdMoto = tag.IdMoto
            };

            AddHateoasLinks(updated, tag.IdTag);

            return Ok(ApiResponse<ArucoTagResource>.Ok(updated, "ArucoTag atualizada com sucesso."));
        }

        // DELETE - Remove uma ArucoTag
        [HttpDelete("{id}", Name = "DeleteArucoTag")]
        [SwaggerOperation(Summary = "Remove uma ArucoTag", Description = "Exclui uma ArucoTag existente pelo ID.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "ArucoTag removida com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "ArucoTag não encontrada")]
        public async Task<IActionResult> DeleteArucoTag(int id)
        {
            var tag = await _context.ArucoTags.FindAsync(id);
            if (tag == null)
                return NotFound(ApiResponse<string>.Fail("ArucoTag não encontrada."));

            _context.ArucoTags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
