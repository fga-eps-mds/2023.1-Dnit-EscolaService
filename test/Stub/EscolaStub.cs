using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Stub
{
    public class EscolaStub
    {
        public CadastroEscolaDTO ObterCadastroEscolaDTO()
        {
            return new CadastroEscolaDTO
            {
                NomeEscola = "DNIT",
                CodigoEscola = 586,
                Cep = "72154365",
                Endereco = "Endereço Teste",
                Latitude = "4654",
                Longitude = "5468",
                NumeroTotalDeAlunos = 400,
                Telefone = "52426252",
                NumeroTotalDeDocentes = 50,
                IdRede = 1,
                IdUf = 1,
                IdLocalizacao = 2,
                IdMunicipio = 1,
                IdEtapasDeEnsino = new List<int> { 2 },
                IdPorte = 2,
                IdSituacao = 1
            };
        }
    }
}
