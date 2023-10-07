
using app.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace test.Stub
{
    public static class AppDbContextExtensions
    {
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
    }
}