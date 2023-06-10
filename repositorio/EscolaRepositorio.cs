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

       
        public Escola ListarInformacoesEscolas(int id_escola)
        {
            var sql = @"SELECT * FROM public.escola WHERE id_escola = @Id_escola";


            var parametro = new
            {
                Id_escola = id_escola
            };

            var escola = contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);

            if (escola == null)
                return null;

            return escola;
        }

        public void AdicionarSituacao(int id_situacao, int id_escola){
            var sql = @"UPDATE public.escola SET id_situacao = @Id_situacao WHERE id_escola = @Id_escola";

            var parametro = new
            {
                Id_situacao = id_situacao,
                Id_escola = id_escola
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);
        }

    }
}
