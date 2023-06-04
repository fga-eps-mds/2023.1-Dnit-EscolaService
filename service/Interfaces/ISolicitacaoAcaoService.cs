using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.Interfaces
{
    public interface ISolicitacaoAcaoService
    {
       public void EnviarSolicitacaoAcao(SolicitacaoAcaoDTO solicitacaoAcaoDTO); 
    }
}
