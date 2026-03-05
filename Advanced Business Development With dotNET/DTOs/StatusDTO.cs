namespace MottuFlow.DTOs
{
    public class StatusDTO
    {
        public string TipoStatus { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataStatus { get; set; }
        public int IdMoto { get; set; }
        public int IdFuncionario { get; set; }
    }
}
