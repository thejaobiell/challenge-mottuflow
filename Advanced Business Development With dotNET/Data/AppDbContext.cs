using Microsoft.EntityFrameworkCore;
using MottuFlowApi.Models;

namespace MottuFlowApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Funcionario> Funcionarios { get; set; } = null!;
        public DbSet<Patio> Patios { get; set; } = null!;
        public DbSet<Moto> Motos { get; set; } = null!;
        public DbSet<Camera> Cameras { get; set; } = null!;
        public DbSet<ArucoTag> ArucoTags { get; set; } = null!;
        public DbSet<Localidade> Localidades { get; set; } = null!;
        public DbSet<RegistroStatus> RegistroStatuses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Esses mapeamentos são opcionais para InMemory, mas funcionam depois que adicionamos o pacote Relational
            modelBuilder.Entity<Funcionario>().ToTable("funcionario");
            modelBuilder.Entity<Patio>().ToTable("patio");
            modelBuilder.Entity<Moto>().ToTable("moto");
            modelBuilder.Entity<Camera>().ToTable("camera");
            modelBuilder.Entity<ArucoTag>().ToTable("aruco_tag");
            modelBuilder.Entity<Localidade>().ToTable("localidade");
            modelBuilder.Entity<RegistroStatus>().ToTable("registro_status");
        }
    }
}
