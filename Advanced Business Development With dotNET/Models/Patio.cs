using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuFlowApi.Models
{
    [Table("patio")]
    public class Patio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_patio")]
        public int IdPatio { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [Column("endereco")]
        public string Endereco { get; set; } = string.Empty;

        [Required]
        [Column("capacidade_maxima")]
        public int CapacidadeMaxima { get; set; }

        // Navigation
        public List<Moto> Motos { get; set; } = new List<Moto>();
        public List<Camera> Cameras { get; set; } = new List<Camera>();
        public List<Localidade> Localidades { get; set; } = new List<Localidade>();
    }
}
