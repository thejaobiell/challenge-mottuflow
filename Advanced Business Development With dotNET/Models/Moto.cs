using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuFlowApi.Models
{
    [Table("moto")]
    public class Moto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_moto")]
        public int IdMoto { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("placa")]
        public string Placa { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("modelo")]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("fabricante")]
        public string Fabricante { get; set; } = string.Empty;

        [Required]
        [Column("ano")]
        public int Ano { get; set; }

        [Required]
        [Column("id_patio")]
        public int IdPatio { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("localizacao_atual")]
        public string LocalizacaoAtual { get; set; } = string.Empty;

        // Navigation
        public Patio? Patio { get; set; }
        public List<ArucoTag> ArucoTags { get; set; } = new List<ArucoTag>();
        public List<Localidade> Localidades { get; set; } = new List<Localidade>();
        public List<RegistroStatus> RegistrosStatus { get; set; } = new List<RegistroStatus>();
    }
}
