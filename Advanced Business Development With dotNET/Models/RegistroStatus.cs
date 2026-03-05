using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuFlowApi.Models
{
    [Table("registro_status")]
    public class RegistroStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_status")]
        public int IdStatus { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("tipo_status")]
        public string TipoStatus { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("descricao")]
        public string? Descricao { get; set; }

        [Required]
        [Column("data_status")]
        public DateTime DataStatus { get; set; } = DateTime.Now;

        [Required]
        [Column("id_moto")]
        public int IdMoto { get; set; }

        [Required]
        [Column("id_funcionario")]
        public int IdFuncionario { get; set; }

        // Navigation
        public Moto? Moto { get; set; }
        public Funcionario? Funcionario { get; set; }
    }
}
