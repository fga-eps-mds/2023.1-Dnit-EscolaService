using dominio.Enums;
using repositorio.Interfaces;
using repositorio.Contexto;
using static repositorio.Contexto.ResolverContexto;
using dominio;
using Dapper;
using CsvHelper;
using System.Collections.Generic;

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

        public bool EscolaJaExiste(int codigoEscola)
        {
            var sqlConsultaEscola = "SELECT COUNT(*) FROM escola WHERE codigo_escola = @CodigoEscola";
            var parametros = new { CodigoEscola = codigoEscola };

            var quantidade = contexto?.Conexao.ExecuteScalar<int>(sqlConsultaEscola, parametros);

            return quantidade > 0;
        }

        public IEnumerable<Escola> Obter()
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
                public.escola";

            var escolas = contexto?.Conexao.Query<Escola>(sql);

            if (escolas == null)
                return null;

            return escolas;

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

        public void AdicionarSituacao(int idSituacao, int idEscola){
            var sql = @"UPDATE public.escola SET id_situacao = @IdSituacao WHERE id_escola = @IdEscola";

            var parametro = new
            {
                IdSituacao = idSituacao,
                IdEscola = idEscola
            };

            contexto?.Conexao.QuerySingleOrDefault<Escola>(sql, parametro);
        }

    }
}
