namespace MottuFlowApi.DTOs
{
    public class PatioOutputDTO
    {
        public int IdPatio { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public int CapacidadeMaxima { get; set; }
    }
}