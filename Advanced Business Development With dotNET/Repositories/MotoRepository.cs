using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Data;
using MottuFlowApi.Models;

namespace MottuFlowApi.Repositories
{
    public class MotoRepository
    {
        private readonly AppDbContext _context;

        public MotoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as motos
        public async Task<List<Moto>> GetAllAsync()
        {
            return await _context.Motos
                                 .Include(m => m.Patio)
                                 .Include(m => m.ArucoTags)
                                 .Include(m => m.RegistrosStatus) 
                                 .Include(m => m.Localidades)
                                 .ToListAsync();
        }

        // Buscar moto por ID
        public async Task<Moto?> GetByIdAsync(int id)
        {
            return await _context.Motos
                                 .Include(m => m.Patio)
                                 .Include(m => m.ArucoTags)
                                 .Include(m => m.RegistrosStatus) 
                                 .Include(m => m.Localidades)
                                 .FirstOrDefaultAsync(m => m.IdMoto == id);
        }

        // Buscar moto por placa
        public async Task<Moto?> GetByPlacaAsync(string placa)
        {
            return await _context.Motos
                                 .Include(m => m.Patio)
                                 .FirstOrDefaultAsync(m => m.Placa == placa);
        }

        // Adicionar moto
        public async Task AddAsync(Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
        }

        // Atualizar moto
        public async Task UpdateAsync(Moto moto)
        {
            _context.Motos.Update(moto);
            await _context.SaveChangesAsync();
        }

        // Remover moto
        public async Task DeleteAsync(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto != null)
            {
                _context.Motos.Remove(moto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
