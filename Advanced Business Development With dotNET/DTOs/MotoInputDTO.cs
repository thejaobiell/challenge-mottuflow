using System.ComponentModel.DataAnnotations;

namespace MottuFlowApi.DTOs
{
    public class MotoInputDTO
    {
        [Required, StringLength(10)]
        public string Placa { get; set; } = null!;

        [Required, StringLength(100)]
        public string Modelo { get; set; } = null!;

        [Required, StringLength(100)]
        public string Fabricante { get; set; } = null!;

        [Range(1900, 2100)]
        public int Ano { get; set; }

        [Required]
        public int IdPatio { get; set; }

        [Required, StringLength(200)]
        public string LocalizacaoAtual { get; set; } = null!;
    }
}


