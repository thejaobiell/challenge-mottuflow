using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Data;
using MottuFlowApi.Models;

namespace MottuFlowApi.Repositories
{
    public class LocalidadeRepository
    {
        private readonly AppDbContext _context;

        public LocalidadeRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as localidades
        public async Task<List<Localidade>> GetAllAsync()
        {
            return await _context.Localidades
                                 .Include(l => l.Moto)
                                 .Include(l => l.Patio)
                                 .Include(l => l.Camera)
                                 .ToListAsync();
        }

        // Buscar localidade por ID
        public async Task<Localidade?> GetByIdAsync(int id)
        {
            return await _context.Localidades
                                 .Include(l => l.Moto)
                                 .Include(l => l.Patio)
                                 .Include(l => l.Camera)
                                 .FirstOrDefaultAsync(l => l.IdLocalidade == id);
        }

        // Adicionar localidade
        public async Task AddAsync(Localidade localidade)
        {
            _context.Localidades.Add(localidade);
            await _context.SaveChangesAsync();
        }

        // Atualizar localidade
        public async Task UpdateAsync(Localidade localidade)
        {
            _context.Localidades.Update(localidade);
            await _context.SaveChangesAsync();
        }

        // Remover localidade
        public async Task DeleteAsync(int id)
        {
            var localidade = await _context.Localidades.FindAsync(id);
            if (localidade != null)
            {
                _context.Localidades.Remove(localidade);
                await _context.SaveChangesAsync();
            }
        }
    }
}
