using System.IO;
using System.Collections.Generic;
using dominio;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public void CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO);
        public ListaPaginada<Escola> Obter(PesquisaEscolaFiltro pesquisaEscolaFiltro);
        public void ExcluirEscola(int id);
        public bool SuperaTamanhoMaximo(MemoryStream planilha);
        public List<int> CadastrarEscolaViaPlanilha(MemoryStream planilha);
        public Escola Listar(int idEscola);
        public void RemoverSituacaoEscola(int idEscola);
        public void AlterarDadosEscola(AtualizarDadosEscolaDTO atualizarDadosEscolaDTO);
    }
}

  


