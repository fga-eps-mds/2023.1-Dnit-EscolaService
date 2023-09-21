using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Entidades
{
    public class Escola
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Nome { get; set; }

        [Required]
        public int Codigo { get; set; }

        [Required, MaxLength(8)]
        public string Cep { get; set; }

        [Required, MaxLength(200)]
        public string Endereco { get; set; }

        [Required, MaxLength(12)]
        public string Latitude { get; set; }

        [Required, MaxLength(12)]
        public string Longitude { get; set; }

        [Required]
        public int TotalAlunos { get; set; }

        [Required]
        public int TotalDocentes { get; set; }

        [Required, MaxLength(11)]
        public string Telefone { get; set; }

        [MaxLength(500)]
        public string Observacao { get; set; }
        
        [Required]
        public int RedeId { get; set; }
        public Rede Rede { get; set; }

        [Required]
        public short UfId { get; set; }
        public UnidadeFederativa Uf { get; set; }

        public int LocalizacaoId { get; set; }
        public Localizacao Localizacao { get; set; }

        public int MunicipioId { get; set; }
        public Municipio Municipio { get; set; }

        public int PorteId { get; set; }
        public Porte Porte { get; set; }

        public int SituacaoId { get; set; }
        public Situacao Situacao { get; set; }

        public List<EtapaEnsino> EtapasEnsino { get; set; }

        [NotMapped]
        public DateTimeOffset AtaualizacaoDate { get; set; }

        public DateTime AtaualizacaoDateUtc
        {
            get => AtaualizacaoDate.UtcDateTime;
            set => AtaualizacaoDate = new DateTimeOffset(value, TimeSpan.Zero);
        }
    }
}