using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MottuFlowApi.Services;
using MottuFlowApi.Models;
using MottuFlowApi.Repositories;

namespace MottuFlow.Tests.Unit
{
    public class FuncionarioServiceUnitTests
    {
        private readonly FuncionarioService _service;
        private readonly Mock<IFuncionarioRepository> _mockRepo;

        public FuncionarioServiceUnitTests()
        {
            // Agora estamos mockando a *interface*, não a classe concreta
            _mockRepo = new Mock<IFuncionarioRepository>();
            _service = new FuncionarioService(_mockRepo.Object);
        }

        [Fact(DisplayName = "Deve retornar lista de funcionários")]
        public async Task ListarTodos_DeveRetornarFuncionarios()
        {
            // Arrange
            var funcionariosMock = new List<Funcionario>
            {
                new Funcionario { Nome = "Léo Mota Lima", CPF = "12345678900" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(funcionariosMock);

            // Act
            var resultado = await _service.ListarTodos();

            // Assert
            Assert.Single(resultado);
            Assert.Equal("Léo Mota Lima", resultado[0].Nome);
        }

        [Fact(DisplayName = "Deve lançar exceção se nome for vazio")]
        public async Task Adicionar_DeveLancarExcecao_SeNomeVazio()
        {
            // Arrange
            var funcionario = new Funcionario { Nome = "" };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.Adicionar(funcionario));
            Assert.Equal("Nome do funcionário é obrigatório.", ex.Message);
        }
    }
}
