using api;
using app.Entidades;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        Task CadastrarAsync(CadastroEscolaDTO cadastroEscolaDTO);
        Task<List<string>> CadastrarAsync(MemoryStream planilha);
        Task AtualizarAsync(Escola escola, EscolaModel data, List<EtapaEnsino>? etapas = null);
        Task<ListaEscolaPaginada<EscolaCorretaModel>> ListarPaginadaAsync(PesquisaEscolaFiltro filtro);
        void ExcluirEscola(int id);
        bool SuperaTamanhoMaximo(MemoryStream planilha);
        void RemoverSituacaoEscola(int idEscola);
        Task<string> ObterCodigoMunicipioPorCEP(string cep);
        int ObterEstadoPelaSigla(string UF);
        int ObterPortePeloId(string Porte);
        int ObterRedePeloId(string Rede);
        int ObterLocalizacaoPeloId(string Localizacao);
        void AlterarDadosEscola(AtualizarDadosEscolaDTO atualizarDadosEscolaDTO);
    }
}