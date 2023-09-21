using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Entidades
{
    public class UnidadeFederativa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required, MaxLength(2)]
        public string Sigla { get; set; }

        [Required, MaxLength(50)]
        public string Descricao { get; set; }
    }
}