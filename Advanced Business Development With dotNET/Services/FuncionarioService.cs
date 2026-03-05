using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MottuFlowApi.Models;
using MottuFlowApi.Repositories;

namespace MottuFlowApi.Services
{
    public class FuncionarioService
    {
        private readonly IFuncionarioRepository _repository;

        public FuncionarioService(IFuncionarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Funcionario>> ListarTodos()
        {
            return await _repository.GetAllAsync();
        }

        public async Task Adicionar(Funcionario funcionario)
        {
            if (string.IsNullOrWhiteSpace(funcionario.Nome))
                throw new Exception("Nome do funcionário é obrigatório.");

            await _repository.AddAsync(funcionario);
        }
    }
}
