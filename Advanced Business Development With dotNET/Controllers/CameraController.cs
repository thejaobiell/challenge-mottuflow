using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Data;
using MottuFlowApi.Models;
using MottuFlowApi.DTOs;
using MottuFlowApi.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuFlowApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cameras")]
    [Tags("Câmeras")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize] // exige JWT para escrita (GET é liberado)
    public class CameraController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CameraController(AppDbContext context) => _context = context;

        // GET - Todas as câmeras (público)
        [HttpGet(Name = "GetCameras")]
        [SwaggerOperation(Summary = "Lista todas as câmeras", Description = "Retorna uma lista paginada de câmeras cadastradas no sistema.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista retornada com sucesso")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]
        public async Task<IActionResult> GetCameras(int page = 1, int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 1);

            var totalItems = await _context.Cameras.CountAsync();

            var cameras = await _context.Cameras
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CameraOutputDTO
                {
                    IdCamera = c.IdCamera,
                    StatusOperacional = c.StatusOperacional,
                    LocalizacaoFisica = c.LocalizacaoFisica,
                    IdPatio = c.IdPatio
                })
                .ToListAsync();

            if (!cameras.Any())
                return Ok(ApiResponse<object>.Ok(new { totalItems = 0, data = new List<CameraOutputDTO>() }, "Nenhuma câmera cadastrada."));

            var meta = new
            {
                totalItems,
                page,
                pageSize,
                totalPages = Math.Ceiling((double)totalItems / pageSize)
            };

            return Ok(ApiResponse<object>.Ok(new { meta, data = cameras }, "Câmeras listadas com sucesso."));
        }

        // GET - Câmera por ID (público)
        [HttpGet("{id}", Name = "GetCamera")]
        [SwaggerOperation(Summary = "Obtém uma câmera específica", Description = "Retorna os detalhes de uma câmera pelo ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Câmera encontrada com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Câmera não encontrada")]
        public async Task<IActionResult> GetCamera(int id)
        {
            var c = await _context.Cameras.FindAsync(id);
            if (c == null)
                return NotFound(ApiResponse<string>.Fail("Câmera não encontrada."));

            var result = new CameraOutputDTO
            {
                IdCamera = c.IdCamera,
                StatusOperacional = c.StatusOperacional,
                LocalizacaoFisica = c.LocalizacaoFisica,
                IdPatio = c.IdPatio
            };

            return Ok(ApiResponse<CameraOutputDTO>.Ok(result, "Câmera encontrada com sucesso."));
        }

        // POST - Criar nova câmera
        [HttpPost(Name = "CreateCamera")]
        [SwaggerOperation(Summary = "Cria uma nova câmera", Description = "Registra uma nova câmera no sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Câmera criada com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação nos dados")]
        public async Task<IActionResult> CreateCamera([FromBody] CameraInputDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var camera = new Camera
            {
                StatusOperacional = input.StatusOperacional,
                LocalizacaoFisica = input.LocalizacaoFisica,
                IdPatio = input.IdPatio
            };

            _context.Cameras.Add(camera);
            await _context.SaveChangesAsync();

            var result = new CameraOutputDTO
            {
                IdCamera = camera.IdCamera,
                StatusOperacional = camera.StatusOperacional,
                LocalizacaoFisica = camera.LocalizacaoFisica,
                IdPatio = camera.IdPatio
            };

            return CreatedAtAction(nameof(GetCamera), new { id = camera.IdCamera },
                ApiResponse<CameraOutputDTO>.Ok(result, "Câmera criada com sucesso."));
        }

        // PUT - Atualizar câmera existente
        [HttpPut("{id}", Name = "UpdateCamera")]
        [SwaggerOperation(Summary = "Atualiza uma câmera existente", Description = "Permite atualizar os dados de uma câmera cadastrada.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Câmera atualizada com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro de validação nos dados")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Câmera não encontrada")]
        public async Task<IActionResult> UpdateCamera(int id, [FromBody] CameraInputDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var camera = await _context.Cameras.FindAsync(id);
            if (camera == null)
                return NotFound(ApiResponse<string>.Fail("Câmera não encontrada."));

            camera.StatusOperacional = input.StatusOperacional;
            camera.LocalizacaoFisica = input.LocalizacaoFisica;
            camera.IdPatio = input.IdPatio;

            _context.Entry(camera).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var updated = new CameraOutputDTO
            {
                IdCamera = camera.IdCamera,
                StatusOperacional = camera.StatusOperacional,
                LocalizacaoFisica = camera.LocalizacaoFisica,
                IdPatio = camera.IdPatio
            };

            return Ok(ApiResponse<CameraOutputDTO>.Ok(updated, "Câmera atualizada com sucesso."));
        }

        // DELETE - Remover câmera
        [HttpDelete("{id}", Name = "DeleteCamera")]
        [SwaggerOperation(Summary = "Remove uma câmera", Description = "Exclui uma câmera do sistema pelo ID.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Câmera removida com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Câmera não encontrada")]
        public async Task<IActionResult> DeleteCamera(int id)
        {
            var c = await _context.Cameras.FindAsync(id);
            if (c == null)
                return NotFound(ApiResponse<string>.Fail("Câmera não encontrada."));

            _context.Cameras.Remove(c);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
