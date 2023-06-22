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


        public void CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO)
        {
            var sqlInserirEscola = @"INSERT INTO public.escola(nome_escola, codigo_escola, cep, endereco, 
            latitude, longitude, numero_total_de_alunos, telefone, numero_total_de_docentes, 
            id_rede, id_uf, id_localizacao, id_municipio, id_etapas_de_ensino, id_porte, id_situacao) 
            VALUES(@Nome, @Codigo, @CEP, @Endereco, @Latitude, 
            @Longitude, @NumeroTotalDeAlunos, Telefone, @NumeroTotalDeDocentes, 
            @IdRede, @IdUf, @IdLocalizacao, @IdMunicipio, @IdEtapasDeEnsino, @IdPorte, @IdSituacao) ";
                
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
