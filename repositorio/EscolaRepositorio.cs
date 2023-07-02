using dominio.Enums;
using repositorio.Interfaces;
using repositorio.Contexto;
using Dapper;
using dominio;
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

        public void CadastrarEscola(Escola escola)
        {
            var sqlInserirEscola = @"INSERT INTO public.escola(nome_escola, codigo_escola, cep, endereco, latitude, longitude, numero_total_de_alunos, telefone,
            numero_total_de_docentes, id_rede, id_uf, id_localizacao, id_municipio, id_etapas_de_ensino, id_porte, id_situacao)
            VALUES(@Nome_escola, @Codigo_escola, @CEP, @Endereco, 
            @Latitude, @Longitude, @Numero_total_de_alunos, @Telefone, @Numero_total_de_docentes, 
            @Id_rede, @Id_uf, @Id_localizacao, @Id_municipio,   
            @Id_etapas_de_ensino, @Id_porte, @Id_situacao)";

            var parametrosEscola = new
            {
                Codigo_escola = escola.CodigoEscola,
                Nome_escola = escola.NomeEscola,
                Id_rede = escola.IdRede,
                CEP = escola.Cep,
                Id_uf = escola.IdUf,
                Endereco = escola.Endereco,
                Id_municipio = escola.IdMunicipio,
                Id_localizacao = escola.IdLocalizacao,
                Longitude = escola.Longitude,
                Latitude = escola.Latitude,
                Id_etapas_de_ensino = escola.IdEtapasDeEnsino,
                Numero_total_de_alunos = escola.NumeroTotalDeAlunos,
                Id_situacao = escola.IdSituacao,
                Id_porte = escola.IdPorte,
                Telefone = escola.Telefone,
                Numero_total_de_docentes = escola.NumeroTotalDeAlunos
            };

            contexto?.Conexao.Execute(sqlInserirEscola, parametrosEscola);
        }

        public int? CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO)
        {

            var sqlInserirEscola = @"INSERT INTO public.escola(nome_escola, codigo_escola, cep, endereco, 
            latitude, longitude, numero_total_de_alunos, telefone, numero_total_de_docentes, 
            id_rede, id_uf, id_localizacao, id_municipio, id_etapas_de_ensino, id_porte, id_situacao) 
            VALUES(@Nome, @Codigo, @CEP, @Endereco, @Latitude, 
            @Longitude, @NumeroTotalDeAlunos, @Telefone, @NumeroTotalDeDocentes, 
            @IdRede, @IdUf, @IdLocalizacao, @IdMunicipio, @IdEtapasDeEnsino, @IdPorte, @IdSituacao) RETURNING id_escola";

            var parametroEscola = new
            {
                Nome = cadastroEscolaDTO.NomeEscola,
                Codigo = cadastroEscolaDTO.CodigoEscola,
                CEP = cadastroEscolaDTO.Cep,
                Endereco = cadastroEscolaDTO.Endereco,
                Latitude = cadastroEscolaDTO.Latitude,
                Longitude = cadastroEscolaDTO.Longitude,
                NumeroTotalDeAlunos = cadastroEscolaDTO.NumeroTotalDeAlunos,
                Telefone = cadastroEscolaDTO.Telefone,
                NumeroTotalDeDocentes = cadastroEscolaDTO.NumeroTotalDeDocentes,
                IdRede = cadastroEscolaDTO.IdRede,
                IdUf = cadastroEscolaDTO.IdUf,
                IdLocalizacao = cadastroEscolaDTO.IdLocalizacao,
                IdMunicipio = cadastroEscolaDTO.IdMunicipio,
                IdEtapasDeEnsino = cadastroEscolaDTO.IdEtapasDeEnsino,
                IdPorte = cadastroEscolaDTO.IdPorte,
                IdSituacao = cadastroEscolaDTO.IdSituacao
            };

            int? idEscola = contexto?.Conexao.ExecuteScalar<int>(sqlInserirEscola, parametroEscola);
            return idEscola;
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
                    LEFT JOIN situacao as s ON e.id_situacao = s.id_situacao
                    LEFT JOIN etapas_de_ensino as ede ON ede.id_etapas_de_ensino = e.id_etapas_de_ensino
                    LEFT JOIN municipio as m ON m.id_municipio = e.id_municipio
                    LEFT JOIN unidade_federativa as uf ON uf.id = e.id_uf ");


            StringBuilder where = new StringBuilder();

            if (pesquisaEscolaFiltro.Nome != null)
                where.Append(" AND e.nome_escola ilike '%' || @NomeEscola || '%' ");
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

            ListaPaginada<Escola> listaEscolaPagina = new(resultados, pesquisaEscolaFiltro.Pagina, pesquisaEscolaFiltro.TamanhoPagina, total ?? 0);

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

            var escola = contexto?.Conexao.QuerySingleOrDefault<Escola?>(sql, parametro);

            if (escola == null)
                throw new InvalidOperationException("Não foi encontrada escola cadastrada com o id fornecido.");

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

        public void AdicionarSituacao(int idSituacao, int idEscola)
        {
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
        public bool EscolaJaExiste(int codigoEscola)
        {
            var sqlConsultaEscola = "SELECT COUNT(*) FROM escola WHERE codigo_escola = @CodigoEscola";
            var parametros = new { CodigoEscola = codigoEscola };

            var quantidade = contexto?.Conexao.ExecuteScalar<int>(sqlConsultaEscola, parametros);

            return quantidade > 0;
        }

        public void AlterarTelefone(int idEscola, string telefone)
        {
            var sql = @"UPDATE public.escola SET telefone = @Telefone WHERE id_escola = @IdEscola";

            var parametro = new
            {
                IdEscola = idEscola,
                Telefone = telefone
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);
        }

        public void AlterarLatitude (int idEscola, string latitude)
        {
            var sql = @"UPDATE public.escola SET latitude = @Latitude WHERE id_escola = @IdEscola";

            var parametro = new
            {
                IdEscola = idEscola,
                Latitude = latitude
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);
        }

        public void AlterarLongitude(int idEscola, string longitude)
        {
            var sql = @"UPDATE public.escola SET longitude = @Longitude WHERE id_escola = @IdEscola";

            var parametro = new
            {
                IdEscola = idEscola,
                Longitude = longitude
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);
        }

        public void AlterarNumeroDeAlunos(int idEscola, int numeroTotalDeAlunos)
        {
            var sql = @"UPDATE public.escola SET numero_total_de_alunos = @NumeroTotalDeAlunos WHERE id_escola = @IdEscola";

            var parametro = new 
            {
                IdEscola = idEscola,
                NumeroTotalDeAlunos = numeroTotalDeAlunos
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);
        }

        public void AlterarNumeroDeDocentes(int idEscola, int numeroTotalDeDocentes)
        {
            var sql = @"UPDATE public.escola SET numero_total_de_docentes = @NumeroTotalDeDocentes WHERE id_escola = @IdEscola";

            var parametro = new 
            {
                IdEscola = idEscola,
                NumeroTotalDeDocentes = numeroTotalDeDocentes
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);
        }
    }
}
