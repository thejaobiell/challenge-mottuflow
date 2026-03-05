using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Models;
using MottuFlowApi.Data;

namespace MottuFlowApi.Repositories
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly AppDbContext _context;

        public FuncionarioRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos os funcionários
        public async Task<List<Funcionario>> GetAllAsync()
        {
            return await _context.Funcionarios
                                 .Include(f => f.RegistrosStatus) // nome correto
                                 .ToListAsync();
        }

        // Buscar funcionário por ID
        public async Task<Funcionario?> GetByIdAsync(int id)
        {
            return await _context.Funcionarios
                                 .Include(f => f.RegistrosStatus) // nome correto
                                 .FirstOrDefaultAsync(f => f.IdFuncionario == id);
        }

        // Buscar por CPF
        public async Task<Funcionario?> GetByCpfAsync(string cpf)
        {
            return await _context.Funcionarios
                                 .FirstOrDefaultAsync(f => f.CPF == cpf); // nome correto
        }

        // Adicionar novo funcionário
        public async Task AddAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();
        }

        // Atualizar funcionário
        public async Task UpdateAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Update(funcionario);
            await _context.SaveChangesAsync();
        }

        // Remover funcionário
        public async Task DeleteAsync(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario != null)
            {
                _context.Funcionarios.Remove(funcionario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
