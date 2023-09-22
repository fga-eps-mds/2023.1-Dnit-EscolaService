using api;
using app.Entidades;
using app.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public int? CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO)
        {
            throw new NotImplementedException();
        }

        public void ExcluirEscola(int Id)
        {
            throw new NotImplementedException();
        }

        public ListaPaginada<EscolaCorretaModel> ObterEscolas(PesquisaEscolaFiltro pesquisaEscolaFiltro)
        {
            throw new NotImplementedException();
        }

        public void RemoverSituacaoEscola(int idEscola)
        {
            throw new NotImplementedException();
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
                Rede = data.Rede,
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
