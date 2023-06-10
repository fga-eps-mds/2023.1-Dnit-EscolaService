using Dapper;
using dominio;
using dominio.Enums;
using repositorio.Contexto;
using repositorio.Interfaces;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
            var sql = @"SELECT * FROM public.escola";

            var escolas = contexto?.Conexao.Query<EscolaCadastrada>(sql);

            if (escolas == null)
                return null;

            return escolas;

        }

        /*
        public EscolaCadastrada ListarEscolasNome(string nome)
        {
            var sql = @"SELECT * FROM public.escola WHERE nome_escola = @Nome";


            var parametro = new
            {
                Nome = nome
            };

            var escola = contexto?.Conexao.QuerySingleOrDefault<EscolaCadastrada>(sql, parametro);

            if (escola == null)
                return null;

            return escola;
        }

        public EscolaCadastrada ListarEscolasEtapas(string etapas_ensino)
        {
            var sql = @"SELECT * FROM public.escola WHERE id_etapas_de_ensino = @EtapasEnsino";


            var parametro = new
            {
                EtapasEnsino = etapas_ensino
            };

            var escola = contexto?.Conexao.QuerySingleOrDefault<EscolaCadastrada>(sql, parametro);

            if (escola == null)
                return null;

            return escola;
        }


        public EscolaCadastrada ListarEscolasUF(string uf)
        {
            var sql = @"SELECT * FROM public.escola WHERE id_uf = @UF";


            var parametro = new
            {
                UF = uf
            };

            var escola = contexto?.Conexao.QuerySingleOrDefault<EscolaCadastrada>(sql, parametro);

            if (escola == null)
                return null;

            return escola;
        }
       
       public EscolaCadastrada ListarEscolasSituacao(string situacao)
       {
           var sql = @"SELECT * FROM public.escola WHERE situacao = @Situacao";


           var parametro = new
           {
               Situacao = situacao
           };

           var escola = contexto?.Conexao.QuerySingleOrDefault<EscolaCadastrada>(sql, parametro);

           if (escola == null)
               return null;

           return escola;
       }

       public EscolaCadastrada ListarEscolasMunicipio(string municipio)
       {
           var sql = @"SELECT * FROM public.escola WHERE id_municipio = @Municipio";


           var parametro = new
           {
               Municipio = municipio
           };

           var escola = contexto?.Conexao.QuerySingleOrDefault<EscolaCadastrada>(sql, parametro);

           if (escola == null)
               return null;

           return escola;
       }
        */
    }

}