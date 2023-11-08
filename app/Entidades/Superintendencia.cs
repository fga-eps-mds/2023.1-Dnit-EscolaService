using api;
using System.ComponentModel.DataAnnotations;

namespace app.Entidades
{
    public class Superintendencia
    {
        [Required]
        public string Endereco { get; set; }  
        
        [Required, MaxLength(10)]
        public string Cep { get; set; }
        
        [Required]
        public string Latitude { get; set; }
        
        [Required]
        public string Longitude { get; set; }
        
        [Required]
        public UF Uf { get; set; }
    }
}

