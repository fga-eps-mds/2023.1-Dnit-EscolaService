using api;
using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace app.Repositorios
{

    public class EscolaRepositorio : IEscolaRepositorio
    {
        private readonly AppDbContext dbContext;

        public EscolaRepositorio(
            AppDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public async Task<Escola> ObterPorIdAsync(Guid id)
        {
            return await dbContext.Escolas.FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new ApiException(ErrorCodes.EscolaNaoEncontrada);
        }

        public EscolaEtapaEnsino AdicionarEtapaEnsino(Escola escola, EtapaEnsino etapa)
        {
            var model = new EscolaEtapaEnsino
            {
                Id = Guid.NewGuid(),
                Escola = escola,
                EtapaEnsino = etapa,
            };
            dbContext.Add(model);
            return model;
        }

        public void ExcluirEscola(int Id)
        {
            throw new NotImplementedException();
        }

        public void RemoverSituacaoEscola(int idEscola)
        {
            throw new NotImplementedException();
        }

        public Escola Criar(CadastroEscolaDTO escolaData, Municipio municipio)
        {
            var escola = new Escola
            {
                Nome = escolaData.NomeEscola,
                Codigo = escolaData.CodigoEscola,
                Cep = escolaData.Cep,
                Endereco = escolaData.Endereco,
                Latitude = escolaData.Latitude,
                Longitude = escolaData.Longitude,
                TotalAlunos = escolaData.NumeroTotalDeAlunos,
                Telefone = escolaData.Telefone,
                TotalDocentes = escolaData.NumeroTotalDeDocentes,
                Rede = (Rede)escolaData.IdRede,
                Uf = (UF)escolaData.IdUf,
                Localizacao = (Localizacao?)escolaData.IdLocalizacao,
                Porte = (Porte?)escolaData.IdPorte,
                Situacao = (Situacao?)escolaData.IdSituacao,
                AtualizacaoDate = DateTimeOffset.Now,
                MunicipioId = municipio.Id,
                Municipio = municipio,
            };
            dbContext.Add(escola);
            return escola;
        }

        public Escola Criar(EscolaModel data)
        {
            var escola = new Escola()
            {
                Id = Guid.NewGuid(),
                Nome = data.NomeEscola,
                Codigo = data.CodigoEscola,
                Cep = data.Cep,
                Endereco = data.Endereco,
                Latitude = data.Latitude,
                Longitude = data.Longitude,
                TotalAlunos = data.NumeroTotalDeAlunos ?? 0,
                Telefone = data.Telefone,
                TotalDocentes = data.NumeroTotalDeDocentes,
                Rede = data.Rede.Value,
                Uf = data.Uf,
                Localizacao = data.Localizacao,
                MunicipioId = data.IdMunicipio,
                Porte = data.Porte,
                Situacao = data.Situacao,
                Observacao = data.Observacao,
                AtualizacaoDate = DateTimeOffset.Now,
            };
            dbContext.Add(escola);
            return escola;
        }

        public void AtualizarDadosPlanilha(EscolaModel escola)
        {
            throw new NotImplementedException();
        }

        public int? AlterarDadosEscola(AtualizarDadosEscolaDTO atualizarDadosEscolaDTO)
        {
            throw new NotImplementedException();
        }

        public void CadastrarEtapasDeEnsino(int idEscola, int idEtapaEnsino)
        {
            throw new NotImplementedException();
        }

        public void RemoverEtapasDeEnsino(int idEscola)
        {
            throw new NotImplementedException();
        }

        public async Task<Escola?> ObterPorCodigoAsync(int codigo)
        {
            return await dbContext.Escolas.Include(e => e.EtapasEnsino).FirstOrDefaultAsync(e => e.Codigo == codigo);
        }
        public bool EscolaJaExiste(int codigoEscola)
        {
            return dbContext.Escolas.Any(e => e.Codigo == codigoEscola);
        }
    }
}
