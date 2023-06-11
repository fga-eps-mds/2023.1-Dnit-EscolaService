namespace dominio
{
    public class Escola
    {
        public int? Id_escola {get; set;}
        public int? codigo_escola { get; set; }
        public string? nome_escola { get; set; }
        public int? Id_rede { get; set; }
        public string? CEP { get; set; }
        public int? Id_uf { get; set; }
        public string? Endereco { get; set; }
        public int? Id_municipio { get; set; }
        public int? Id_localizacao { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public int? Id_etapas_de_ensino { get; set;} 
        public int? Numero_total_de_alunos { get; set; }
        public int? Id_situacao { get; set; }
        public int? Id_porte { get; set; }
        public string? Telefone { get; set; }
        public int? Numero_total_de_docentes{ get; set; }
    }
}