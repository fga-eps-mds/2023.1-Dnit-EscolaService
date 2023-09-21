using System.ComponentModel.DataAnnotations;

namespace app.Entidades
{
    public class EscolaEtapaEnsino
    {
        [Required]
        public Guid EscolaId { get; set; }
        public Escola Escola { get; set; }

        [Required]
        public int EtapaEnsinoId { get; set; }
        public EtapaEnsino EtapaEnsino { get; set; }
    }
}