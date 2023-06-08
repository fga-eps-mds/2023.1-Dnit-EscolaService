using Dapper;
using dominio;
using dominio.Enums;
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

       
        public Escola ListarInformacoesEscolas(string nome)
        {
            var sql = @"SELECT * FROM public.escola WHERE nome = @Nome";


            var parametro = new
            {
                Nome = nome
            };

            var escola = contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);

            if (escola == null)
                return null;

            return escola;
        }

        public void AdicionarSituacao(string situacao){
            var sql = @"INSERT INTO public.escola (situacao) VALUES (@Situacao)";


            var parametro = new
            {
                Situacao = situacao
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);
        }

    }
}
