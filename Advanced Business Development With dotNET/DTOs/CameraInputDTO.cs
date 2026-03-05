namespace MottuFlowApi.DTOs
{
    public class CameraInputDTO
    {
        public string StatusOperacional { get; set; } = string.Empty;
        public string LocalizacaoFisica { get; set; } = string.Empty;
        public int IdPatio { get; set; }
    }
}
