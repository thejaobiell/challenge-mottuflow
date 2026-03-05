using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuFlowApi.Models
{
    [Table("camera")]
    public class Camera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_camera")]
        public int IdCamera { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("status_operacional")]
        public string StatusOperacional { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("localizacao_fisica")]
        public string LocalizacaoFisica { get; set; } = string.Empty;

        [Required]
        [Column("id_patio")]
        public int IdPatio { get; set; }

        // Navigation
        public Patio? Patio { get; set; }
        public List<Localidade> Localidades { get; set; } = new List<Localidade>();
    }
}
