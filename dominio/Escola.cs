namespace dominio
{
    public class Escola
    {
        public int IdEscola { get; set; }
        public int CodigoEscola { get; set; }
        public string NomeEscola { get; set; }
        public int? IdRede { get; set; }
        public string DescricaoRede {get; set; }
        public string Cep { get; set; }
        public int? IdUf { get; set; }
        public string DescricaoUf { get; set; }
        public string Endereco { get; set; }
        public int? IdMunicipio { get; set; }
        public string NomeMunicipio { get; set; }
        public int? IdLocalizacao { get; set; }
        public string DescricaoLocalizacao { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int? IdEtapasDeEnsino { get; set;} 
        public string DescricaoEtapasEnsino { get; set; }
        public int? NumeroTotalDeAlunos { get; set; }
        public int? IdSituacao { get; set; }
        public string DescricaoSituacao { get; set; }
        public int? IdPorte { get; set; }
        public string Telefone { get; set; }
        public int NumeroTotalDeDocentes{ get; set; }
        public string SiglaUf { get; set; }
        public string Observacao {get; set; }
    }
}