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


        public void CadastrarEscola(Escola escola)
        {
            var sqlInserirEscola = @"INSERT INTO public.escola(nome_escola, codigo_escola, cep, endereco, 
            latitude, longitude, numero_total_de_alunos, telefone, numero_total_de_docentes, id_escola, 
            id_rede, id_uf, id_localizacao, id_municipio, id_etapas_de_ensino, id_porte, id_situacao) 
            VALUES(@nome_escola, @codigo_escola, @CEP, @Endereco, @Latitude, 
            @Longitude, @numeroTotalDeAlunos, Telefone, @numeroTotalDeDocentes, @idEscola, 
            @idRede, @idUf, @idLocalizacao, @idMunicipio, @idEtapasDeEnsino, @idPorte, @idSituacao) ";
                
            var parametroEscola = new
            {
                Nome = escola.NomeEscola,
                Codigo = escola.CodigoEscola,
                CEP = escola.Cep,
                Endereco = escola.Endereco,
                Latitude = escola.Latitude,
                Longitude = escola.Longitude,
                NumeroTotalDeAlunos = escola.NumeroTotalDeAlunos,
                Telefone = escola.Telefone,
                NumeroTotalDeDocentes = escola.NumeroTotalDeDocentes,
                IdEscola = escola.IdEscola,
                IdRede = escola.IdRede,
                IdUf = escola.IdUf,
                Localizacao = escola.IdLocalizacao,
                IdMunicipio = escola.IdMunicipio,
            };
            int? EscolaId = contexto?.Conexao.ExecuteScalar<int>(sqlInserirEscola, parametroEscola);
        }

        public Escola ListarInformacoesEscola(int idEscola)
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
