namespace MottuFlowApi.DTOs
{
    public class LocalidadeInputDTO
    {
        public DateTime DataHora { get; set; } = DateTime.Now;
        public string PontoReferencia { get; set; } = string.Empty;
        public int IdMoto { get; set; }
        public int IdPatio { get; set; }
        public int IdCamera { get; set; }
    }
}