using System.Globalization;
using api;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace app.Entidades
{
    public class AppDbContext : DbContext
    {
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Escola> Escolas { get; set; }
        public DbSet<EscolaEtapaEnsino> EscolaEtapaEnsino { get; set; }
        public DbSet<Superintendencia> Superintendencias { get; set; }
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options)
        { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Escola>().HasMany(escola => escola.EtapasEnsino).WithOne(e => e.Escola);
        }

        public void Popula()
        {
            PopulaMunicipiosPorArquivo(null, Path.Join(".", "Migrations", "Data", "municipios.csv"));
            
            PopulaSuperintendenciasPorArquivo(Path.Join(".", "Migrations", "Data", "superintendencias.csv"));
        }

        public List<Municipio>? PopulaMunicipiosPorArquivo(int? limit, string caminho)
        {
            var hasMunicipio = Municipios.Any();
            var municipios = new List<Municipio>();

            if (hasMunicipio)
            {
                return null;
            }

            using (var fs = File.OpenRead(caminho))
            using (var parser = new TextFieldParser(fs))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                var columns = new Dictionary<string, int> { { "id", 0 }, { "name", 1 }, { "uf", 2 } };

                while (!parser.EndOfData)
                {
                    var row = parser.ReadFields()!;
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
            AddRange(municipios);
            SaveChanges();
            return municipios;
        }

        public List<Superintendencia>? PopulaSuperintendenciasPorArquivo(string caminho)
        {
            var hasSuperintendencias = Superintendencias.Any();
            if(hasSuperintendencias) return null;
            
            var superintendencias = new List<Superintendencia>();
            using (var streamReader = new StreamReader(caminho))
            using (var csvReader = new CsvReader(streamReader, new CsvConfiguration(new CultureInfo("pt-BR")){Delimiter = ";"}) )
            {
                superintendencias = csvReader.GetRecords<Superintendencia>().ToList();
            }    
            AddRange(superintendencias);
            SaveChanges();
            return superintendencias;
        }
        
    }
    
}