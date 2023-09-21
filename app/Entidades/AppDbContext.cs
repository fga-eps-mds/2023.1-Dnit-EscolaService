
using dominio.Dominio;
using Microsoft.EntityFrameworkCore;

namespace app.Entidades
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Escola> Escolas { get; set; }
        public DbSet<EscolaEtapaEnsino> EscolaEtapaEnsino { get; set; }

        public AppDbContext (IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSql"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Escola>().HasMany(escola => escola.EtapasEnsino).WithOne(e => e.Escola);
        }
    }
}