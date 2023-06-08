using Dapper;
using dominio;
using repositorio.Contexto;
using repositorio.Interfaces;
using System.Collections.Generic;
using static repositorio.Contexto.ResolverContexto;

namespace repositorio
{
    public class EscolaRepositorio : IEscolaRepositorio
    {
        private readonly IContexto contexto;

        public EscolaRepositorio(ResolverContextoDelegate resolverContexto)
        {
            contexto = resolverContexto(ContextoBancoDeDados.Postgresql);
        }


        public EscolasCadastradas VisualizarEscolas()
        {
            var sql = @"SELECT * FROM public.escolas";

            var escolas = contexto?.Conexao.QuerySingleOrDefault<UsuarioDnit>(sql, parametro);

            if (escolas == null)
                return null;

            return escolas;
        }

    }
}