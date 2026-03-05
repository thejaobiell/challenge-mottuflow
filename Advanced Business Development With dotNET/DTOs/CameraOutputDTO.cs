namespace MottuFlowApi.DTOs
{
    public class CameraOutputDTO
    {
        public int IdCamera { get; set; }
        public string StatusOperacional { get; set; } = string.Empty;
        public string LocalizacaoFisica { get; set; } = string.Empty;
        public int IdPatio { get; set; }
    }
}