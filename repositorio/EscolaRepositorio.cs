using dominio.Enums;
using repositorio.Interfaces;
using repositorio.Contexto;
using static repositorio.Contexto.ResolverContexto;
using dominio;
using Dapper;

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
                Codigo_escola = escola.codigo_escola,
                Nome_escola = escola.nome_escola,
                Id_rede = escola.Id_rede,
                CEP = escola.CEP,
                Id_uf = escola.Id_uf,
                Endereco = escola.Endereco,
                Id_municipio = escola.Id_municipio,
                Id_localizacao = escola.Id_localizacao,
                Longitude = escola.Longitude,
                Latitude = escola.Latitude,
                Id_etapas_de_ensino = escola.Id_etapas_de_ensino,
                Numero_total_de_alunos = escola.Numero_total_de_alunos,
                Id_situacao = escola.Id_situacao,
                Id_porte = escola.Id_porte,
                Telefone = escola.Telefone,
                Numero_total_de_docentes = escola.Numero_total_de_docentes
            };

            contexto?.Conexao.Execute(sqlInserirEscola, parametrosEscola);
        }

        
    }


}
