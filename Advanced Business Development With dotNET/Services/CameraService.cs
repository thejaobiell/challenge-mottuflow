using MottuFlowApi.Models;
using MottuFlowApi.Repositories;

namespace MottuFlowApi.Services
{
    public class CameraService
    {
        private readonly CameraRepository _repository;

        public CameraService(CameraRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Camera>> ListarTodas()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Camera?> ObterPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task Adicionar(Camera camera)
        {
            if (string.IsNullOrWhiteSpace(camera.StatusOperacional))
                throw new Exception("Status operacional é obrigatório");

            if (string.IsNullOrWhiteSpace(camera.LocalizacaoFisica))
                throw new Exception("Localização física é obrigatória");

            await _repository.AddAsync(camera);
        }

        public async Task Atualizar(Camera camera)
        {
            await _repository.UpdateAsync(camera);
        }

        public async Task Remover(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
