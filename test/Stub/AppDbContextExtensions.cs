
using app.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace test.Stub
{
    public static class AppDbContextExtensions
    {
        private static List<Municipio>? municipios;

        public static List<Escola> SeedEscolas(this AppDbContext dbContext, int limite = 1, bool comEtapas = true)
        {
            var escolas = new List<Escola>();
            
            if (!dbContext.Municipios.Any())
            {
                dbContext.SeedMunicipios(limite);
            }
            
            var municipios = dbContext.Municipios.Take(1).ToList();

            foreach (var escola in EscolaStub.ListarEscolas(municipios, comEtapas).Take(limite))
            {
                dbContext.Add(escola);
                escolas.Add(escola);
            }
            dbContext.SaveChanges();
            return escolas;
        }

        public static List<Municipio>? SeedMunicipios(this AppDbContext dbContext, int limit)
        {
            if (municipios != default && limit < municipios.Count)
            {
                dbContext.AddRange(municipios.Take(limit).ToList());
                dbContext.SaveChanges();
                return municipios;
            }
            var caminho = Path.Join("..", "..", "..", "Stub", "municipios.csv");
            municipios = dbContext.SeedMunicipiosPorArquivo(limit, caminho);
            return municipios;
        }
    }
}