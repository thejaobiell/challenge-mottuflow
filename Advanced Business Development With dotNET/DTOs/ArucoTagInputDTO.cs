namespace MottuFlowApi.DTOs
{
    public class ArucoTagInputDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int IdMoto { get; set; }
    }
}