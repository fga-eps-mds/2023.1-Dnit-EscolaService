using api;
using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace app.Repositorios
{

    public class RanqueRepositorio : IRanqueRepositorio
    {
        private readonly AppDbContext dbContext;

        public RanqueRepositorio(
            AppDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public async Task<Ranque?> ObterPorIdAsync(int id)
        {
            return await dbContext.Ranques.FindAsync(id);
        }

        public async Task<Ranque?> ObterUltimoRanqueAsync()
        {
            return await dbContext.Ranques
                .Where(r => r.DataFimUtc != null)
                .OrderByDescending(r => r.DataFimUtc)
                .FirstOrDefaultAsync();
        }

        public async Task<Ranque?> ObterRanqueEmProcessamentoAsync()
        {
            return await dbContext.Ranques
                .OrderByDescending(r => r.DataFimUtc)
                .FirstOrDefaultAsync();
        }

        public async Task<ListaPaginada<EscolaRanque>> ListarEscolasAsync(int ranqueId, PesquisaEscolaFiltro filtro)
        {
            var query = dbContext.EscolaRanques
                .Include(er => er.Ranque)
                .Include(er => er.Escola).ThenInclude(e => e.EtapasEnsino)
                .Include(er => er.Escola).ThenInclude(e => e.Municipio)
                .Where(er => er.RanqueId == ranqueId);

            if (filtro.Nome != null)
            {
                query = query.Where(er => er.Escola.Nome.ToLower().Contains(filtro.Nome.Trim().ToLower()));
            }
            if (filtro.IdEtapaEnsino != null)
            {
                var etapas = filtro.IdEtapaEnsino.ConvertAll(e => (EtapaEnsino)e);
                query = query.Where(er => er.Escola.EtapasEnsino!.Any(etapa => etapas.Contains(etapa.EtapaEnsino)));
            }
            if (filtro.IdMunicipio != null)
            {
                query = query.Where(er => er.Escola.MunicipioId == filtro.IdMunicipio);
            }
            if (filtro.IdUf != null)
            {
                query = query.Where(er => er.Escola.Uf == (UF)filtro.IdUf);
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(er => er.Pontuacao)
                .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina)
                .Take(filtro.TamanhoPagina)
                .ToListAsync();

            return new ListaPaginada<EscolaRanque>(items, filtro.Pagina, filtro.TamanhoPagina, total);
        }

        // https://stackoverflow.com/questions/56482415/entity-framework-6-calculate-numeric-index-position-of-row-in-a-given-table-s
        public async Task<(EscolaRanque?, int)> ObterEscolaRanqueEPosicaoPorIdAsync(Guid escolaId, int ranqueId)
        {
            var escola = await dbContext.EscolaRanques
                .Where(er => er.EscolaId == escolaId && er.RanqueId == ranqueId)
                .FirstOrDefaultAsync();

            Console.WriteLine(">>> Pontucao " + escola!.Pontuacao);

            // var posicao = await dbContext.EscolaRanques
            //     .OrderByDescending(er => er.Pontuacao)
            //     .Where(er => er.RanqueId == ranqueId && er.Pontuacao >= escola!.Pontuacao)
            //     .Select((row, i) => new AlgumaCoisa { RanqueId = row.RanqueId, Pontuacao = row.Pontuacao, Posicao = i })
            //     // .Where(er => )
            //     // .Where(er => er.EscolaId == escolaId)
            //     // .FirstAsync()
            //     .ToListAsync()
            //     ;
            var posicao = 1;
            return (escola, posicao);
        }
    }

    class AlgumaCoisa
    {
        public int RanqueId { get; set; }
        public int Pontuacao { get; set; }
        public int Posicao { get; set; }
    }
}
