using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api
{
    public class PesquisaEscolaFiltro
    {
        public string? Nome { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int? IdUf { get; set; }
        public int? IdSituacao { get; set; }
        public List<int>? IdEtapaEnsino { get; set; }
        public int? IdMunicipio { get; set; }
    }
}
