using System.ComponentModel.DataAnnotations;

namespace MottuFlowApi.DTOs
{
    public class FuncionarioInputDTO
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(11, MinimumLength = 11)]
        public string Cpf { get; set; } = null!;

        [Required, StringLength(50)]
        public string Cargo { get; set; } = null!;

        [Required, StringLength(15)]
        public string Telefone { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 6)]
        public string Senha { get; set; } = null!;
    }
}
