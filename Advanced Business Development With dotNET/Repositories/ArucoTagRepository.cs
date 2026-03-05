using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Data;
using MottuFlowApi.Models;

namespace MottuFlowApi.Repositories
{
    public class ArucoTagRepository
    {
        private readonly AppDbContext _context;

        public ArucoTagRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as tags
        public async Task<List<ArucoTag>> GetAllAsync()
        {
            return await _context.ArucoTags
                                 .Include(a => a.Moto)
                                 .ToListAsync();
        }

        // Buscar tag por ID
        public async Task<ArucoTag?> GetByIdAsync(int id)
        {
            return await _context.ArucoTags
                                 .Include(a => a.Moto)
                                 .FirstOrDefaultAsync(a => a.IdTag == id);
        }

        // Buscar tag por código
        public async Task<ArucoTag?> GetByCodigoAsync(string codigo)
        {
            return await _context.ArucoTags
                                 .Include(a => a.Moto)
                                 .FirstOrDefaultAsync(a => a.Codigo == codigo);
        }

        // Adicionar tag
        public async Task AddAsync(ArucoTag tag)
        {
            _context.ArucoTags.Add(tag);
            await _context.SaveChangesAsync();
        }

        // Atualizar tag
        public async Task UpdateAsync(ArucoTag tag)
        {
            _context.ArucoTags.Update(tag);
            await _context.SaveChangesAsync();
        }

        // Remover tag
        public async Task DeleteAsync(int id)
        {
            var tag = await _context.ArucoTags.FindAsync(id);
            if (tag != null)
            {
                _context.ArucoTags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
}
