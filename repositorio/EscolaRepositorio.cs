using Dapper;
using dominio;
using dominio.Enums;
using repositorio.Contexto;
using repositorio.Interfaces;
using System;
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


        public IEnumerable<EscolaCadastrada> ObterEscolas()
        {
            throw new System.NotImplementedException();
        }
        public void ExcluirEscola(int id)
        {
            var sql = @"DELETE FROM public.escola WHERE id_escola = @IdEscola";

            var parametro = new
            {
                IdEscola = id,
            };

            contexto?.Conexao.Execute(sql, parametro);

        }
    }
}