using MottuFlowApi.Models;
using MottuFlowApi.Repositories;

namespace MottuFlowApi.Services
{
    public class LocalidadeService
    {
        private readonly LocalidadeRepository _repository;

        public LocalidadeService(LocalidadeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Localidade>> ListarTodas()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Localidade?> ObterPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task Adicionar(Localidade localidade)
        {
            if (string.IsNullOrWhiteSpace(localidade.PontoReferencia))
                throw new Exception("Ponto de referência é obrigatório");

            if (localidade.IdMoto <= 0 || localidade.IdPatio <= 0 || localidade.IdCamera <= 0)
                throw new Exception("IDs de Moto, Pátio e Câmera devem ser válidos");

            await _repository.AddAsync(localidade);
        }

        public async Task Atualizar(Localidade localidade)
        {
            await _repository.UpdateAsync(localidade);
        }

        public async Task Remover(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
