namespace MottuFlowApi.DTOs
{
    public class ArucoTagOutputDTO
    {
        public int IdTag { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int IdMoto { get; set; }
    }
}