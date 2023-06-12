using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using System.Collections.Generic;

namespace service.Interfaces
{
    public interface IEscolaService
    {
        public Escola ListarInformacoesEscola(int idEscola);

        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO);

    }
}
