
using app.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace test.Stub
{
    public static class AppDbContextExtensions
    {
        public static List<Escola> SeedEscolas(this AppDbContext dbContext, int limit = 1)
        {
            var escolas = new List<Escola>();
            
            if (!dbContext.Municipios.Any())
            {
                dbContext.SeedMunicipios(limit);
            }
            
            var municipios = dbContext.Municipios.Take(1).ToList();

            foreach (var escola in EscolaStub.ListarEscolas(municipios).Take(limit))
            {
                dbContext.Add(escola);
            }
            dbContext.SaveChanges();
            return escolas;
        }
    }
}