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

        public IEnumerable<Escola> Obter()
        {
            string[] colunas = {
                    "escola.nome_escola nomeEscola", "escola.codigo_escola codigoEscola", "escola.cep", "escola.endereco",
                    "escola.latitude", "escola.longitude", "escola.numero_total_de_alunos numeroTotalDeAlunos", "escola.telefone",
                    "escola.numero_total_de_docentes numeroTotalDeDocentes", "escola.id_escola idEscola",
                    "escola.id_rede idRede", "rede.descricao_rede descricaoRede",
                    "escola.id_uf idUf", "uf.sigla siglaUf",
                    "escola.id_localizacao idLocalizacao", "localizacao.descricao_localizacao descricaoLocalizacao",
                    "escola.id_municipio idMunicipio", "municipio.nome nomeMunicipio",
                    "escola.id_etapas_de_ensino idEtapasDeEnsino", "etapas.descricao_etapas_de_ensino descricaoEtapasDeEnsino",
                    "escola.id_porte idPorte", "porte.descricao_porte descricaoPorte",
                    "escola.id_situacao idSituacao", "situacao.descricao_situacao descricaoSituacao"
            };
            var queryBuilder = new QueryBuilder();
            var sql = queryBuilder.Select(colunas)
                        .From("public.escola")
                        .Join("public.rede rede", "escola.id_rede", "rede.id_rede")
                        .Join("public.unidade_federativa uf", "escola.id_uf", "uf.id")
                        .Join("public.localizacao localizacao", "escola.id_localizacao", "localizacao.id_localizacao")
                        .Join("public.municipio municipio", "escola.id_municipio", "municipio.id_municipio")
                        .Join("public.etapas_de_ensino etapas", "escola.id_etapas_de_ensino", "etapas.id_etapas_de_ensino")
                        .Join("public.porte porte", "escola.id_porte", "porte.id_porte")
                        .Join("public.situacao situacao", "escola.id_situacao", "situacao.id_situacao")
                        .Build();

            var escolas = contexto?.Conexao.Query<Escola>(sql);

            if (escolas == null)
                return null;

            return escolas;


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

       
        public Escola Obter(int idEscola)
        {
            string[] colunas = {
                    "escola.nome_escola nomeEscola", "escola.codigo_escola codigoEscola", "escola.cep", "escola.endereco",
                    "escola.latitude", "escola.longitude", "escola.numero_total_de_alunos numeroTotalDeAlunos", "escola.telefone",
                    "escola.numero_total_de_docentes numeroTotalDeDocentes", "escola.id_escola idEscola",
                    "escola.id_rede idRede", "rede.descricao_rede descricaoRede",
                    "escola.id_uf idUf", "uf.sigla siglaUf",
                    "escola.id_localizacao idLocalizacao", "localizacao.descricao_localizacao descricaoLocalizacao",
                    "escola.id_municipio idMunicipio", "municipio.nome nomeMunicipio",
                    "escola.id_etapas_de_ensino idEtapasDeEnsino", "etapas.descricao_etapas_de_ensino descricaoEtapasDeEnsino",
                    "escola.id_porte idPorte", "porte.descricao_porte descricaoPorte",
                    "escola.id_situacao idSituacao", "situacao.descricao_situacao descricaoSituacao"
            };
            var queryBuilder = new QueryBuilder();
            var sql = queryBuilder.Select(colunas)
                        .From("public.escola")
                        .Join("public.rede rede", "escola.id_rede", "rede.id_rede")
                        .Join("public.unidade_federativa uf", "escola.id_uf", "uf.id")
                        .Join("public.localizacao localizacao", "escola.id_localizacao", "localizacao.id_localizacao")
                        .Join("public.municipio municipio", "escola.id_municipio", "municipio.id_municipio")
                        .Join("public.etapas_de_ensino etapas", "escola.id_etapas_de_ensino", "etapas.id_etapas_de_ensino")
                        .Join("public.porte porte", "escola.id_porte", "porte.id_porte")
                        .Join("public.situacao situacao", "escola.id_situacao", "situacao.id_situacao")
                        .Where("escola.id_escola", "@IdEscola")
                        .Build();

            var parametro = new
            {
                IdEscola = idEscola
            };

            var escola = contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);

            if (escola == null)
                return null;
            return escola;
        }

        public void AdicionarSituacao(int idSituacao, int idEscola){
            var sql = @"UPDATE public.escola SET id_situacao = @IdSituacao WHERE id_escola = @IdEscola";

            var parametro = new
            {
                IdSituacao = idSituacao,
                IdEscola = idEscola
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);
        }
        public void RemoverSituacaoEscola(int idEscola)
        {
            var sql = @"UPDATE public.escola SET id_situacao = NULL WHERE id_escola = @IdEscola";

            var parametro = new
            {
                IdEscola = idEscola
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro); 
        }
    }
}
