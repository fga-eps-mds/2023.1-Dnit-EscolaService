using api;
using app.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace test.Stub
{
    public static class EscolaStub
    {
        public static IEnumerable<Escola> ListarEscolas(IEnumerable<Municipio> municipios, bool comEtapas)
        {
            while (true)
            {
                var escola = new Escola
                {
                    Id = Guid.NewGuid(),
                    AtualizacaoDate = DateTime.Now,
                    Cep = $"7215436{Random.Shared.Next() % 10}",
                    Endereco = $"Endereço Teste {Random.Shared.Next()}",
                    Codigo = Random.Shared.Next() % 1000,
                    Latitude = Random.Shared.NextDouble().ToString().Truncate(12),
                    Longitude = Random.Shared.NextDouble().ToString().Truncate(12),
                    Localizacao = Enum.GetValues<Localizacao>().TakeRandom(true).FirstOrDefault(),
                    Municipio = municipios.TakeRandom().First(),
                    Nome = $"Escola DNIT {Random.Shared.Next()}",
                    Porte = Enum.GetValues<Porte>().TakeRandom(true).FirstOrDefault(),
                    Rede = Enum.GetValues<Rede>().TakeRandom(true).FirstOrDefault(),
                    Situacao = Enum.GetValues<Situacao>().TakeRandom(true).FirstOrDefault(),
                    Telefone = "52426252",
                    TotalAlunos = Random.Shared.Next() % 100 + 1,
                    TotalDocentes = Random.Shared.Next() % 100 + 1,
                    Uf = Enum.GetValues<UF>().TakeRandom().FirstOrDefault(),
                };
                if (comEtapas)
                {
                    escola.EtapasEnsino = Enum.GetValues<EtapaEnsino>().TakeRandom().ConvertAll(etapa => new EscolaEtapaEnsino
                    {
                        Id = Guid.NewGuid(),
                        EscolaId = escola.Id,
                        Escola = escola,
                        EtapaEnsino = etapa,
                    });
                }
                yield return escola;
            }
        }
    }
}
