using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuFlowApi.Models
{
    [Table("funcionario")]
    public class Funcionario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_funcionario")]
        public int IdFuncionario { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        [Column("cpf")]
        public string CPF { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("cargo")]
        public string Cargo { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("telefone")]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("senha")]
        public string Senha { get; set; } = string.Empty;

        // Navigation
        public List<RegistroStatus> RegistrosStatus { get; set; } = new List<RegistroStatus>();
    }
}
