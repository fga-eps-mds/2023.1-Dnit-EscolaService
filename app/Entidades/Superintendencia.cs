using api;
using System.ComponentModel.DataAnnotations;

namespace app.Entidades
{
    public class Superintendencia
    {
        public string Endereco { get; set; }  
        
        [Required]
        public string Latitude { get; set; }
        
        [Required]
        public string Longitude { get; set; }
        
        [MaxLength(10)]
        public string Cep { get; set; }
        
        [Key]
        public int Id { get; set; }
        
        [Required]
        public UF Uf { get; set; }
    }
}

