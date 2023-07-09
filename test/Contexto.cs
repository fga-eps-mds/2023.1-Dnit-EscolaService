using System.Data;
using repositorio.Contexto;
using Dapper;

namespace test
{
    public class Contexto : IContexto
    {
        public IDbConnection Conexao { get; }
        public Contexto(IDbConnection conexao)
        {
            Conexao = conexao;

            string sql = @"
                ATTACH DATABASE ':memory:' AS public;

                CREATE TABLE public.etapas_de_ensino (
                    descricao_etapas_de_ensino TEXT,
                    id_etapas_de_ensino INTEGER PRIMARY KEY AUTOINCREMENT
                );

                CREATE TABLE public.unidade_federativa (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    sigla TEXT,
                    descricao TEXT
                );

                CREATE TABLE public.municipio (
                    nome INTEGER,
                    id_uf INTEGER,
                    id_municipio INTEGER PRIMARY KEY AUTOINCREMENT
                );

                CREATE TABLE public.situacao (
                    id_situacao INTEGER PRIMARY KEY AUTOINCREMENT,
                    descricao_situacao TEXT
                );
                  
                CREATE TABLE public.escola_etapas_de_ensino (
                    id_escola INTEGER,
                    id_etapas_de_ensino INTEGER
                );

                CREATE TABLE public.rede (
                    id_rede INTEGER PRIMARY KEY AUTOINCREMENT,
                    descricao_rede TEXT
                );

                CREATE TABLE public.localizacao (
                    id_localizacao INTEGER PRIMARY KEY AUTOINCREMENT,
                    descricao_localizacao TEXT
                );

                CREATE TABLE public.escola(nome_escola TEXT, codigo_escola INTEGER, cep TEXT, endereco TEXT,
                    latitude TEXT, longitude TEXT, numero_total_de_alunos INTEGER, telefone TEXT, numero_total_de_docentes INTEGER,
                    id_escola INTEGER PRIMARY KEY AUTOINCREMENT, id_rede INTEGER, id_uf INTEGER, id_localizacao INTEGER,
                    id_municipio INTEGER, id_etapas_de_ensino INTEGER, id_porte INTEGER, id_situacao INTEGER, observacao TEXT, ultima_atualizacao DATETIME
                );

                INSERT INTO public.etapas_de_ensino(descricao_etapas_de_ensino)
                VALUES ('Educação Infantil'), ('Ensino Fundamental');

                INSERT INTO public.situacao(descricao_situacao)
                VALUES ('A'), ('B');

                INSERT INTO public.unidade_federativa(sigla, descricao)
                VALUES ('DF', 'Distrito Federal'), ('GO', 'Goiás');

                INSERT INTO public.municipio(nome, id_uf)
                VALUES ('Brasília', 1), ('Goiânia', 2);

                INSERT INTO public.escola (nome_escola, codigo_escola, cep, endereco, latitude, longitude, numero_total_de_alunos,
                                           telefone, numero_total_de_docentes, id_rede, id_uf, id_localizacao, id_municipio,
                                           id_etapas_de_ensino, id_porte, id_situacao)
                VALUES ('CEM02', 2, '234567', '345678', '25897', '7132649', 20, '33333333', 15, 1, 1, 2, 1, 1, 2, 1),
                       ('CEM04', 4, '891245', '7012364', '9214589', '8412971', 19, '5555555', 21, 1, 1, 1, 1, 1, 1, 1),
                       ('CEM03', 3, '264462', '5848692', '897891', '124569', 15, '4444444', 18, 2, 1, 2, 1, 1, 2, 2);
            ";


            Conexao.Execute(sql);
        }
    }
}
