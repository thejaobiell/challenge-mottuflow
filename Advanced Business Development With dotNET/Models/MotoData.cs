using Microsoft.ML.Data;

namespace MottuFlowApi.Models
{
    public class MotoData
    {
        [LoadColumn(0)]
        public float Vibracao { get; set; }

        [LoadColumn(1)]
        public float TemperaturaMotor { get; set; }

        [LoadColumn(2)]
        public float KMRodados { get; set; }

        [LoadColumn(3)]
        public float IdadeOleoDias { get; set; }

        [LoadColumn(4), ColumnName("Label")]
        public bool PrecisaManutencao { get; set; }
    }

    public class ManutencaoPredicao
    {
        [ColumnName("PredictedLabel")]
        public bool Predicao { get; set; }

        public float Score { get; set; }

        public float Probabilidade { get; set; }
    }
}
