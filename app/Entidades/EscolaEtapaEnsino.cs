using api;
using System.ComponentModel.DataAnnotations;

namespace app.Entidades
{
    public class EscolaEtapaEnsino
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EscolaId { get; set; }
        public Escola Escola { get; set; }

        [Required]
        public EtapaEnsino EtapaEnsino { get; set; }
    }
}