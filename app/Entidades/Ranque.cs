using api;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Entidades
{
    public class Ranque
    {
        [Key]
        public int Id { get; set; }
        
        [NotMapped]
        public DateTimeOffset DataInicio { get; set; }

        public DateTime DataInicioUtc
        {
            get => DataInicio.UtcDateTime;
            set => DataInicio = new DateTimeOffset(value, TimeSpan.Zero);
        }
        
        [NotMapped]
        public DateTimeOffset? DataFim { get; set; }

        public DateTime? DataFimUtc
        {
            get => DataFim?.UtcDateTime;
            set => DataFim = value != null ? new DateTimeOffset(value.Value, TimeSpan.Zero) : null;
        }
    }
}