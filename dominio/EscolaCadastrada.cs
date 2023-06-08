namespace dominio
{
    public class EscolaCadastrada
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Rede { get; set; }
        public string CEP { get; set; }
        public string UF { get; set; }
        public string Endereco { get; set; }
        public string Municipio { get; set; }
        public string Localizacao { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string EtapasEnsino { get; set;} 
        public int TotalAlunos { get; set; }
        public string Situacao { get; set; }
        public string Porte { get; set; }
        public int Telefone { get; set; }
        public int TotalDocentes{ get; set; }

    }
}