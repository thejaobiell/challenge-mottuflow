namespace MottuFlowApi.DTOs
{
    public class LocalidadeOutputDTO
    {
        public int IdLocalidade { get; set; }
        public DateTime DataHora { get; set; }
        public string PontoReferencia { get; set; } = string.Empty;
        public int IdMoto { get; set; }
        public int IdPatio { get; set; }
        public int IdCamera { get; set; }
    }
}