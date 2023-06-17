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
