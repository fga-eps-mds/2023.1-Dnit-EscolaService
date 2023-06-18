using dominio;

namespace test.Stub
{
    public class SolicitacaoAcaoStub
    {
        public SolicitacaoAcaoDTO ObterSolicitacaoAcaoDTO()
        {
            return new SolicitacaoAcaoDTO
            {
                Escola = "Escola Teste",
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
