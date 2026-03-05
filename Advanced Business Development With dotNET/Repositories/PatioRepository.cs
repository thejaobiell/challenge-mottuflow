using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Data;
using MottuFlowApi.Models;

namespace MottuFlowApi.Repositories
{
    public class PatioRepository
    {
        private readonly AppDbContext _context;

        public PatioRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos os pátios
        public async Task<List<Patio>> GetAllAsync()
        {
            return await _context.Patios
                                 .Include(p => p.Motos)
                                 .Include(p => p.Cameras)
                                 .Include(p => p.Localidades)
                                 .ToListAsync();
        }

        // Buscar pátio por ID
        public async Task<Patio?> GetByIdAsync(int id)
        {
            return await _context.Patios
                                 .Include(p => p.Motos)
                                 .Include(p => p.Cameras)
                                 .Include(p => p.Localidades)
                                 .FirstOrDefaultAsync(p => p.IdPatio == id);
        }

        // Adicionar pátio
        public async Task AddAsync(Patio patio)
        {
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();
        }

        // Atualizar pátio
        public async Task UpdateAsync(Patio patio)
        {
            _context.Patios.Update(patio);
            await _context.SaveChangesAsync();
        }

        // Remover pátio
        public async Task DeleteAsync(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio != null)
            {
                _context.Patios.Remove(patio);
                await _context.SaveChangesAsync();
            }
        }
    }
}
