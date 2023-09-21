
using dominio.Dominio;
using Microsoft.EntityFrameworkCore;

namespace app.Entidades
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DbSet<UnidadeFederativa> UnidadesFederativas { get; set; }
        public DbSet<EtapaEnsino> EtapasEnsino { get; set; }
        public DbSet<Situacao> Situacao { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Rede> Redes { get; set; }
        public DbSet<Porte> Portes { get; set; }
        public DbSet<Localizacao> Localizacoes { get; set; }
        public DbSet<Escola> Escolas { get; set; }

        public AppDbContext (IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSql"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UnidadeFederativa>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<EtapaEnsino>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Situacao>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Rede>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Porte>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Localizacao>().Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}