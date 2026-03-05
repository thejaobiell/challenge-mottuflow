using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuFlowApi.Models
{
    [Table("aruco_tag")]
    public class ArucoTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_tag")]
        public int IdTag { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("codigo")]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Required]
        [Column("id_moto")]
        public int IdMoto { get; set; }

        // Navigation
        public Moto? Moto { get; set; }
    }
}
