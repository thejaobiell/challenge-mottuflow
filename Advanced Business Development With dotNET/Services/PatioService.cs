using MottuFlowApi.Models;
using MottuFlowApi.Repositories;

namespace MottuFlowApi.Services
{
    public class PatioService
    {
        private readonly PatioRepository _repository;

        public PatioService(PatioRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Patio>> ListarTodos()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Patio?> ObterPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task Adicionar(Patio patio)
        {
            if (string.IsNullOrWhiteSpace(patio.Nome))
                throw new Exception("Nome do pátio é obrigatório");

            if (patio.CapacidadeMaxima <= 0)
                throw new Exception("Capacidade máxima deve ser maior que zero");

            await _repository.AddAsync(patio);
        }

        public async Task Atualizar(Patio patio)
        {
            await _repository.UpdateAsync(patio);
        }

        public async Task Remover(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
