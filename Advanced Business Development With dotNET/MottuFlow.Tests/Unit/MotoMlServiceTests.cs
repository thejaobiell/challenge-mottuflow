using Microsoft.ML;
using MottuFlowApi.Models;
using MottuFlowApi.Services;
using System.IO;
using Xunit;

public class MotoMlServiceTests
{
    private readonly MotoMlService mlService;

    public MotoMlServiceTests()
    {
        var contexto = new MLContext();
        var arquivoDados = Path.Combine("Scripts", "ml.csv");
        var raizProjeto = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
        var arquivoModelo = Path.Combine(raizProjeto, "ml.zip");

        // se ml.zip existir ele será usado, caso contrário será criado
        mlService = new MotoMlService(contexto, arquivoDados, arquivoModelo);
    }

    [Theory]
    [InlineData(0.2, 85.5, 1500, 30)]
    [InlineData(0.6, 98.5, 8000, 120)]
    public void Prever_RetornaResultadoValido(float vibracao, float tempMotor, float km, float idadeOleo)
    {
        var moto = new MotoData
        {
            Vibracao = vibracao,
            TemperaturaMotor = tempMotor,
            KMRodados = km,
            IdadeOleoDias = idadeOleo
        };

        var resultado = mlService.Prever(moto);

        Assert.NotNull(resultado);
        Assert.InRange(resultado.Probabilidade, 0, 1);
    }
}
