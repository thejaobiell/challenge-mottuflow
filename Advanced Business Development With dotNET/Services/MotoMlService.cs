using Microsoft.ML;
using Microsoft.ML.Data;
using MottuFlowApi.Models;
using System;
using System.IO;

namespace MottuFlowApi.Services
{
    public class MotoMlService
    {
        private readonly MLContext contexto;
        private readonly ITransformer modelo;
        private readonly string arquivoDados;
        private readonly string arquivoModelo;

        public MotoMlService(MLContext contexto, string? arquivoDados = null, string? arquivoModelo = null)
        {
            this.contexto = contexto;
            this.arquivoDados = arquivoDados ?? Path.Combine("Scripts", "ml.csv");
            this.arquivoModelo = arquivoModelo ?? "ml.zip";

            modelo = TreinarESalvarModelo();
        }

        public ManutencaoPredicao Prever(MotoData moto)
        {
            var previsao = contexto.Model.CreatePredictionEngine<MotoData, ManutencaoPredicao>(modelo);
            return previsao.Predict(moto);
        }

        private ITransformer TreinarESalvarModelo()
        {
            if (!File.Exists(arquivoDados))
                throw new FileNotFoundException($"Arquivo de dados não encontrado: {arquivoDados}");

            var dados = contexto.Data.LoadFromTextFile<MotoData>(
                path: arquivoDados,
                hasHeader: true,
                separatorChar: ',');

            var pipeline = contexto.Transforms
                .Concatenate("Features",
                    nameof(MotoData.Vibracao),
                    nameof(MotoData.TemperaturaMotor),
                    nameof(MotoData.KMRodados),
                    nameof(MotoData.IdadeOleoDias))
                .Append(contexto.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "Label",
                    featureColumnName: "Features"));

            var modeloTreinado = pipeline.Fit(dados);

            contexto.Model.Save(modeloTreinado, dados.Schema, arquivoModelo);

            return modeloTreinado;
        }
    }
}
