using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class SolicitacaoAcaoDTO
    {
        public string Escola { get; set; }
        public string NomeSolicitante { get; set; }
        public string VinculoEscola { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CiclosEnsino { get; set; }
        public int QuantidadeAlunos { get; set; }
        public string Observacoes { get; set; }
    }
}
