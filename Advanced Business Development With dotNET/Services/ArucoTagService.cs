using MottuFlowApi.Models;
using MottuFlowApi.Repositories;

namespace MottuFlowApi.Services
{
    public class ArucoTagService
    {
        private readonly ArucoTagRepository _repository;

        public ArucoTagService(ArucoTagRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ArucoTag>> ListarTodas()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ArucoTag?> ObterPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ArucoTag?> ObterPorCodigo(string codigo)
        {
            return await _repository.GetByCodigoAsync(codigo);
        }

        public async Task Adicionar(ArucoTag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.Codigo))
                throw new Exception("Código da tag é obrigatório");

            if (string.IsNullOrWhiteSpace(tag.Status))
                throw new Exception("Status da tag é obrigatório");

            await _repository.AddAsync(tag);
        }

        public async Task Atualizar(ArucoTag tag)
        {
            await _repository.UpdateAsync(tag);
        }

        public async Task Remover(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
