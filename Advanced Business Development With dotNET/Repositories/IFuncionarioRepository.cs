using System.Collections.Generic;
using System.Threading.Tasks;
using MottuFlowApi.Models;

namespace MottuFlowApi.Repositories
{
    public interface IFuncionarioRepository
    {
        Task<List<Funcionario>> GetAllAsync();
        Task<Funcionario?> GetByIdAsync(int id);
        Task<Funcionario?> GetByCpfAsync(string cpf);
        Task AddAsync(Funcionario funcionario);
        Task UpdateAsync(Funcionario funcionario);
        Task DeleteAsync(int id);
    }
}
