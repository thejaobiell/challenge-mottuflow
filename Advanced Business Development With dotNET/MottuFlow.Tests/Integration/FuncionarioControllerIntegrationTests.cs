using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MottuFlowApi.Data;
using Xunit;

namespace MottuFlow.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class FuncionarioControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public FuncionarioControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "Banco InMemory deve estar criado e acessível")]
        public void BancoDeveEstarCriado()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            bool podeConectar = db.Database.CanConnect();
            int total = db.Funcionarios.Count();

            Assert.True(podeConectar, "O banco InMemory não pôde ser acessado.");
            Assert.True(total > 0, "Nenhum funcionário foi inicializado no banco InMemory.");

            Console.WriteLine($"Banco InMemory acessível com {total} funcionário(s).");
        }

        [Fact(DisplayName = "GET /api/v1/funcionarios deve retornar 200 OK")]
        public async Task GetFuncionarios_DeveRetornarStatus200()
        {
            // Login com usuário válido criado no banco InMemory
            var loginPayload = new
            {
                Username = "leo@mottuflow.com",
                Password = "123456"
            };

            var responseLogin = await _client.PostAsJsonAsync("/api/v1/auth/login", loginPayload);

            // Exibe erro detalhado caso o login falhe
            var loginContent = await responseLogin.Content.ReadAsStringAsync();
            if (!responseLogin.IsSuccessStatusCode)
                throw new Exception($"Falha no login: {responseLogin.StatusCode} - {loginContent}");

            var loginResult = await responseLogin.Content.ReadFromJsonAsync<LoginResponse>();
            Assert.NotNull(loginResult);
            Assert.False(string.IsNullOrEmpty(loginResult!.Token), "Token JWT não foi retornado.");

            // Configurar token JWT no header Authorization
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResult.Token);

            // Executar GET no endpoint protegido
            var response = await _client.GetAsync("/api/v1/funcionarios");
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                Console.WriteLine($"Erro ao requisitar endpoint: {content}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Léo Mota Lima", content);

            Console.WriteLine(" Endpoint /api/v1/funcionarios retornou 200 OK com sucesso.");
        }

        private class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}
