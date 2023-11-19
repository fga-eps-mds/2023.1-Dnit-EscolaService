﻿using app.Controllers;
using app.Entidades;
using System.IO;
using System.Linq;
using test.Fixtures;
using test.Stubs;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace test
{
    public class DbContextText : TestBed<Base>, IDisposable
    {
        private readonly AppDbContext dbContext;

        public DbContextText(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
        }

        [Fact]
        public void Popula_QuandoNaoExistirMunicipio_DevePopular()
        {
            dbContext.PopulaMunicipiosPorArquivo(5, Path.Join("..", "..", "..", "..", "app", "Migrations", "Data", "municipios.csv"));

            Assert.Equal(5, dbContext.Municipios.Count());
        }

        [Fact]
        public void Popula_QuandoNaoExistirSuperintendencia_DevePopular()
        {
            dbContext.PopulaSuperintendenciasPorArquivo(5, Path.Join("..", "..", "..", "..", "app", "Migrations", "Data", "superintendencias.csv"));

            Assert.Equal(5, dbContext.Superintendencias.Count());
        }

        public new void Dispose()
        {
            dbContext.Clear();
        }
    }
}
