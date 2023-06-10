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
        public int? Excluir(int Id)
        {
            var sqlBuscarDados = @"DELETE FROM public.escola WHERE id_escola = @IdEscola RETURNING id_escola";

            var parametro = new
            {
                IdEscola = Id,
            };

            int? IdEscola = contexto?.Conexao.Execute(sqlBuscarDados, parametro);
            return IdEscola;
        }
    }
}