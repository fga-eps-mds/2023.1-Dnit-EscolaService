using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Escolas
{
    public class CadastroEscolaDTO
    {
        public int CodigoEscola { get; set; }
        public string? NomeEscola { get; set; }
        public int IdRede { get; set; }
        public string? Cep { get; set; }
        public int IdUf { get; set; }
        public string? Endereco { get; set; }
        public int? IdMunicipio { get; set; }
        public int? IdLocalizacao { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public List<int>? IdEtapasDeEnsino { get; set; }
        public int NumeroTotalDeAlunos { get; set; }
        public int? IdSituacao { get; set; }
        public int? IdPorte { get; set; }
        public string? Telefone { get; set; }
        public int NumeroTotalDeDocentes { get; set; }
        public DateTime UltimaAtualizacao { get; set; }

    }
}