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
                numero_total de docentes numeroTotalDeDocentes,
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

            var escolas = contexto?.Conexao.Query<EscolaCadastrada>(sql);

            if (escolas == null)
                return null;

            return escolas;

        }
    }

}