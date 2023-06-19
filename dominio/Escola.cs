namespace dominio
{
    public class Escola
    {
        public string NomeEscola { get; set; }
        public int CodigoEscola { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int NumeroTotalDeAlunos { get; set; }
        public string Telefone { get; set; }
        public int NumeroTotalDeDocentes{ get; set; }
        public int IdEscola { get; set; }
        public int IdRede { get; set; }
        public int IdUf { get; set; }
        public int IdLocalizacao { get; set; }
        public int IdMunicipio { get; set; }
        public int IdEtapasDeEnsino { get; set;} 
        public int IdPorte { get; set; }
        public int IdSituacao { get; set; }
    }
}