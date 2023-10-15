using api.Escolas;

namespace test.Stubs
{
    public class SolicitacaoAcaoStub
    {
        public SolicitacaoAcaoDTO ObterSolicitacaoAcaoDTO()
        {
            return new SolicitacaoAcaoDTO
            {
                Escola = "Escola Teste",
                UF = "DF",
                Municipio = "Brasília",
                NomeSolicitante = "João Testador",
                VinculoEscola = "Professor",
                Email = "joao@email.com",
                Telefone = "123123123",
                CiclosEnsino = new[] { "Ensino Médio", "Ensino Fundamental" },
                QuantidadeAlunos = 503,
                Observacoes = "Teste de Solicitação"
            };
        }
    }
}
