using System.ComponentModel.DataAnnotations;

namespace MottuFlowApi.DTOs
{
    public class PatioInputDTO
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Endereco { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "A capacidade deve ser maior que 0.")]
        public int CapacidadeMaxima { get; set; }
    }
}
