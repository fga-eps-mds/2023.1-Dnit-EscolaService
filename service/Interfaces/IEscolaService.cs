using System.IO;
using System.Collections.Generic;
using dominio;
using System.Threading.Tasks;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public void CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO);
        public ListaPaginada<Escola> Obter(PesquisaEscolaFiltro pesquisaEscolaFiltro);
        public void ExcluirEscola(int id);
        public bool SuperaTamanhoMaximo(MemoryStream planilha);
        public List<string> CadastrarEscolaViaPlanilha(MemoryStream planilha);
        public Escola Listar(int idEscola);
        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO);
        public void RemoverSituacaoEscola(int idEscola);
        public Task<string> ObterCodigoMunicipioPorCEP(string cep);
        public int ObterEstadoPelaSigla(string UF);
        public int ObterPortePeloId(string Porte);
        public int ObterRedePeloId(string Rede);
        public int ObterLocalizacaoPeloId(string Localizacao);
    }

}

  


