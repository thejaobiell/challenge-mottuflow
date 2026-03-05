using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Data;
using MottuFlowApi.Models;

namespace MottuFlowApi.Repositories
{
    public class CameraRepository
    {
        private readonly AppDbContext _context;

        public CameraRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as câmeras
        public async Task<List<Camera>> GetAllAsync()
        {
            return await _context.Cameras
                                 .Include(c => c.Patio)
                                 .Include(c => c.Localidades)
                                 .ToListAsync();
        }

        // Buscar câmera por ID
        public async Task<Camera?> GetByIdAsync(int id)
        {
            return await _context.Cameras
                                 .Include(c => c.Patio)
                                 .Include(c => c.Localidades)
                                 .FirstOrDefaultAsync(c => c.IdCamera == id);
        }

        // Adicionar câmera
        public async Task AddAsync(Camera camera)
        {
            _context.Cameras.Add(camera);
            await _context.SaveChangesAsync();
        }

        // Atualizar câmera
        public async Task UpdateAsync(Camera camera)
        {
            _context.Cameras.Update(camera);
            await _context.SaveChangesAsync();
        }

        // Remover câmera
        public async Task DeleteAsync(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera != null)
            {
                _context.Cameras.Remove(camera);
                await _context.SaveChangesAsync();
            }
        }
    }
}
