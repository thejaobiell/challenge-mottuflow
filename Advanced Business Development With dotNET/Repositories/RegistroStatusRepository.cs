using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Data;
using MottuFlowApi.Models;

namespace MottuFlowApi.Repositories
{
    public class RegistroStatusRepository
    {
        private readonly AppDbContext _context;

        public RegistroStatusRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos os registros de status
        public async Task<List<RegistroStatus>> GetAllAsync()
        {
            return await _context.RegistroStatuses
                                 .Include(r => r.Moto)
                                 .Include(r => r.Funcionario)
                                 .ToListAsync();
        }

        // Buscar registro por ID
        public async Task<RegistroStatus?> GetByIdAsync(int id)
        {
            return await _context.RegistroStatuses
                                 .Include(r => r.Moto)
                                 .Include(r => r.Funcionario)
                                 .FirstOrDefaultAsync(r => r.IdStatus == id);
        }

        // Adicionar registro
        public async Task AddAsync(RegistroStatus registro)
        {
            _context.RegistroStatuses.Add(registro);
            await _context.SaveChangesAsync();
        }

        // Atualizar registro
        public async Task UpdateAsync(RegistroStatus registro)
        {
            _context.RegistroStatuses.Update(registro);
            await _context.SaveChangesAsync();
        }

        // Remover registro
        public async Task DeleteAsync(int id)
        {
            var registro = await _context.RegistroStatuses.FindAsync(id);
            if (registro != null)
            {
                _context.RegistroStatuses.Remove(registro);
                await _context.SaveChangesAsync();
            }
        }
    }
}
