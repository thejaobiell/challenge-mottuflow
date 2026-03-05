using MottuFlowApi.Models;
using MottuFlowApi.Repositories;

namespace MottuFlowApi.Services
{
    public class MotoService : IMotoService
    {
        private readonly MotoRepository _repository;

        public MotoService(MotoRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Moto>> GetPagedAsync(int page, int size)
        {
            var allMotos = await _repository.GetAllAsync();
            var items = allMotos.Skip((page - 1) * size).Take(size).ToList();

            return new PagedResult<Moto>
            {
                PageSize = size,
                TotalCount = allMotos.Count,
                Items = items
            };
        }

        public async Task<Moto> GetByIdAsync(int id)
        {
            var moto = await _repository.GetByIdAsync(id);
            if (moto == null)
                throw new Exception("Moto não encontrada");

            return moto;
        }

        public async Task<Moto> CreateAsync(Moto moto)
        {
            if (string.IsNullOrWhiteSpace(moto.Placa))
                throw new Exception("Placa é obrigatória");

            if (string.IsNullOrWhiteSpace(moto.Modelo))
                throw new Exception("Modelo é obrigatório");

            if (moto.Ano <= 0)
                throw new Exception("Ano da moto inválido");

            await _repository.AddAsync(moto);
            return moto;
        }

        public async Task<Moto> UpdateAsync(Moto moto)
        {
            await _repository.UpdateAsync(moto);
            return moto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var moto = await _repository.GetByIdAsync(id);
            if (moto == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
