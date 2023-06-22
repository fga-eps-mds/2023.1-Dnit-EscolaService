using Dapper;
using dominio.Dominio;
using dominio.Enums;
using repositorio.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static repositorio.Contexto.ResolverContexto;

namespace repositorio
{
    public class DominioRepositorio
    {
        private readonly IContexto contexto;

        public DominioRepositorio(ResolverContextoDelegate resolverContexto)
        {
            contexto = resolverContexto(ContextoBancoDeDados.Postgresql);
        }
        public IEnumerable<UnidadeFederativa> ObterUnidadeFederativa()
        {
            var sql = @"SELECT id, sigla, descricao FROM public.unidade_federativa";

            var unidadesFederativas = contexto?.Conexao.Query<UnidadeFederativa>(sql);

            return unidadesFederativas ?? Enumerable.Empty<UnidadeFederativa>();
        }
        public IEnumerable<EtapasdeEnsino> ObterEtapasdeEnsino()
        {
            var sql = @"SELECT id, sigla, descricao FROM public.etapas_de_ensino";

            var EtapasdeEnsino = contexto?.Conexao.Query<EtapasdeEnsino>(sql);

            return EtapasdeEnsino ?? Enumerable.Empty<EtapasdeEnsino>();
        }
        public IEnumerable<Municipio> ObterMunicipio()
        {
            var sql = @"SELECT id, sigla, descricao FROM public.municipio";

            var Municipio = contexto?.Conexao.Query<Municipio>(sql);

            return Municipio ?? Enumerable.Empty<Municipio>();
        }
        public IEnumerable<Situacao> ObterSituacao()
        {
            var sql = @"SELECT id, sigla, descricao FROM public.situcao";

            var Situacao = contexto?.Conexao.Query<Situacao>(sql);

            return Situacao ?? Enumerable.Empty<Situacao>();
        }

    }
}
