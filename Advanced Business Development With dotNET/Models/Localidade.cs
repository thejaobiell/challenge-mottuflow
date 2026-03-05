using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuFlowApi.Models
{
    [Table("localidade")]
    public class Localidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_localidade")]
        public int IdLocalidade { get; set; }

        [Required]
        [Column("data_hora")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(100)]
        [Column("ponto_referencia")]
        public string PontoReferencia { get; set; } = string.Empty;

        [Required]
        [Column("id_moto")]
        public int IdMoto { get; set; }

        [Required]
        [Column("id_patio")]
        public int IdPatio { get; set; }

        [Required]
        [Column("id_camera")]
        public int IdCamera { get; set; }

        // Navigation
        public Moto? Moto { get; set; }
        public Patio? Patio { get; set; }
        public Camera? Camera { get; set; }
    }
}
