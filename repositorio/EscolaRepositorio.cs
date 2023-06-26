using Dapper;
using dominio;
using dominio.Enums;
using repositorio.Contexto;
using repositorio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public ListaPaginada<Escola> ObterEscolas(PesquisaEscolaFiltro pesquisaEscolaFiltro)
        {

            StringBuilder sql = new(@$"
                SELECT e.nome_escola as NomeEscola,
		            e.codigo_escola as CodigoEscola, 
		            e.cep as Cep,
		            e.endereco as Endereco, 
		            e.latitude as Latitude,
		            e.longitude as Longitude,
                    e.numero_total_de_alunos as NumeroTotalDeAlunos,
	                e.telefone as Telefone, 
	                e.numero_total_de_docentes as NumeroTotalDeDocentes,
                    e.id_escola as IdEscola,
	                e.id_rede as IdRede,
	                e.id_uf as IdUf,
	                e.id_localizacao as IdLocalizacao,
	                e.id_municipio as IdMunicipio,
                    e.id_etapas_de_ensino as IdEtapasDeEnsino,
	                e.id_porte as IdPorte,
	                e.id_situacao as IdSituacao,
                    s.descricao_situacao as DescricaoSituacao,
	                ede.descricao_etapas_de_ensino as DescricaoEtapasEnsino,
	                m.nome as NomeMunicipio,
	                uf.descricao as DescricaoUf,
                    uf.sigla as SiglaUf
                FROM public.escola as e
                    JOIN situacao as s ON e.id_situacao = s.id_situacao
                    JOIN etapas_de_ensino as ede ON ede.id_etapas_de_ensino = e.id_etapas_de_ensino
                    JOIN municipio as m ON m.id_municipio = e.id_municipio
                    JOIN unidade_federativa as uf ON uf.id = e.id_uf ");


            StringBuilder where = new StringBuilder();

            if (pesquisaEscolaFiltro.Nome != null)
                where.Append(" AND e.nome_escola like '%' || @NomeEscola || '%' ");
            if (pesquisaEscolaFiltro.IdSituacao != null)
                where.Append(" AND e.id_situacao  = @IdSituacao");
            if (pesquisaEscolaFiltro.IdEtapaEnsino != null)
                where.Append(" AND e.id_etapas_de_ensino = @IdEtapasEnsino");
            if (pesquisaEscolaFiltro.IdMunicipio != null)
                where.Append(" AND e.id_municipio = @IdMunicipio");
            if (pesquisaEscolaFiltro.IdUf != null)
                where.Append(" AND e.id_uf = @IdUf");

            if (where.Length > 0)
            {
                sql.Append(" WHERE ");
                sql.Append(where.ToString().TrimStart(' ', 'A', 'N', 'D', ' '));
            }

            sql.Append(" ORDER BY e.nome_escola");
           
            var parametros = new
            {
                Pagina = pesquisaEscolaFiltro.Pagina,
                TamanhoPagina = pesquisaEscolaFiltro.TamanhoPagina,
                NomeEscola = pesquisaEscolaFiltro.Nome,
                IdSituacao = pesquisaEscolaFiltro.IdSituacao,
                IdEtapasEnsino = pesquisaEscolaFiltro.IdEtapaEnsino,
                IdMunicipio = pesquisaEscolaFiltro.IdMunicipio,
                IdUf = pesquisaEscolaFiltro.IdUf
            };

            var resultados = contexto?.Conexao.Query<Escola>(sql.ToString(), parametros);

            int? total = resultados.Count();
            resultados = resultados.Skip((pesquisaEscolaFiltro.Pagina - 1) * pesquisaEscolaFiltro.TamanhoPagina).Take(pesquisaEscolaFiltro.TamanhoPagina);

            ListaPaginada<Escola> listaEscolaPagina = new(resultados,pesquisaEscolaFiltro.Pagina, pesquisaEscolaFiltro.TamanhoPagina, total ?? 0);

            return listaEscolaPagina;


        }
        public Escola Obter(int idEscola)
        {
            var sql = @"
                SELECT
                    nome_escola nomeEscola,
                    codigo_escola codigoEscola,
                    cep,
                    endereco,
                    latitude,
                    longitude,
                    numero_total_de_alunos numeroTotalDeAlunos,
                    telefone,
                    numero_total_de_docentes numeroTotalDeDocentes,
                    id_escola idEscola,
                    id_rede idRede,
                    id_uf idUf,
                    id_localizacao idLocalizacao,
                    id_municipio idMunicipio,
                    id_etapas_de_ensino idEtapasDeEnsino,
                    id_porte idPorte,
                    id_situacao idSituacao
                FROM
                    public.escola
                WHERE
                    id_escola = @IdEscola";

            var parametro = new
            {
                IdEscola = idEscola
            };

            var escola = contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);

            if (escola == null)
                return null;
            return escola;
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
