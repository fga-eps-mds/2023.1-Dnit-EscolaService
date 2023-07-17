using System.IO;
using System.Collections.Generic;
using dominio;
using System.Threading.Tasks;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public void CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO);
        public ListaPaginada<EscolaCorreta> Obter(PesquisaEscolaFiltro pesquisaEscolaFiltro);
        public void ExcluirEscola(int id);
        public bool SuperaTamanhoMaximo(MemoryStream planilha);
        public List<string> CadastrarEscolaViaPlanilha(MemoryStream planilha);
        public void RemoverSituacaoEscola(int idEscola);
        public Task<string> ObterCodigoMunicipioPorCEP(string cep);
        public int ObterEstadoPelaSigla(string UF);
        public int ObterPortePeloId(string Porte, string nomeEscola);
        public int ObterRedePeloId(string Rede);
        public int ObterLocalizacaoPeloId(string Localizacao);
        public List<int> EtapasParaIds(string etapas, string nomeEscola);
        public void AlterarDadosEscola(AtualizarDadosEscolaDTO atualizarDadosEscolaDTO);
    }
}