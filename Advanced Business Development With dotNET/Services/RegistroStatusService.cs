using MottuFlowApi.Models;
using MottuFlowApi.Repositories;

namespace MottuFlowApi.Services
{
    public class RegistroStatusService
    {
        private readonly RegistroStatusRepository _repository;

        public RegistroStatusService(RegistroStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RegistroStatus>> ListarTodos()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<RegistroStatus?> ObterPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task Adicionar(RegistroStatus registro)
        {
            if (string.IsNullOrWhiteSpace(registro.TipoStatus))
                throw new Exception("Tipo de status é obrigatório");

            if (registro.IdFuncionario <= 0 || registro.IdMoto <= 0)
                throw new Exception("IDs de Funcionário e Moto devem ser válidos");

            await _repository.AddAsync(registro);
        }

        public async Task Atualizar(RegistroStatus registro)
        {
            await _repository.UpdateAsync(registro);
        }

        public async Task Remover(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
