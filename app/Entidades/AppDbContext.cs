
using api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualBasic.FileIO;

namespace app.Entidades
{
    public class AppDbContext : DbContext
    {
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Escola> Escolas { get; set; }
        public DbSet<EscolaEtapaEnsino> EscolaEtapaEnsino { get; set; }

        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options)
        { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Escola>().HasMany(escola => escola.EtapasEnsino).WithOne(e => e.Escola);
        }

        public void Seed()
        {
            SeedMunicipios();
        }

        private List<Municipio> SeedMunicipios(int? limit = null)
        {
            var hasMunicipio = Municipios.Any();
            var municipios = new List<Municipio>();

            if (hasMunicipio)
            {
                return null;
            }

            using (var fs = File.OpenRead(Path.Join(".", "Migrations", "Data", "municipios.csv")))
            using (var parser = new TextFieldParser(fs))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                var columns = new Dictionary<string, int> { { "id", 0 }, { "name", 1 }, { "uf", 2 } };

                while (!parser.EndOfData)
                {
                    var row = parser.ReadFields();
                    var municipio = new Municipio
                    {
                        Id = int.Parse(row[columns["id"]]),
                        Nome = row[columns["name"]],
                        Uf = (UF)int.Parse(row[columns["uf"]]),
                    };

                    municipios.Add(municipio);
                    if (limit.HasValue && municipios.Count >= limit.Value)
                    {
                        break;
                    }
                }
            }
            SaveChanges();
            return municipios;
        }
    }
}