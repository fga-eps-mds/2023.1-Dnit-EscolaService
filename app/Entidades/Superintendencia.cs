using api;
using System.ComponentModel.DataAnnotations;

namespace app.Entidades
{
    public class Superintendencia
    {
        [Required]
        public string endereco { get; set; }  
        
        [Required, MaxLength(10)]
        public string cep { get; set; }
        
        [Required]
        public string latitude { get; set; }
        
        [Required]
        public string longitude { get; set; }
        
        [Required]
        public UF Uf { get; set; }
    }
}

