namespace dominio
{
    public class EscolasCadastradas
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public string rede { get; set; }
        public string CEP { get; set; }
        public string UF { get; set; }
        public string endereco { get; set; }
        public string municipio { get; set; }
        public string localizacao { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string etapas_ensino { get; set;} 
        public int total_alunos { get; set; }
        public string situacao { get; set; }
        public string porte { get; set; }
        public string situacao { get; set; }
        public int telefone { get; set; }
        public int total_docentes{ get; set; }

   public (int codigo, string nome, string rede, string cep, string UF, string endereco, string municipio,
                   string localizacao, string longitude, string latitude, string etapasEnsino, int total_alunos,
                   string situacao, string porte, int telefone, int total_docentes)
        {
            this.Codigo = codigo;
            this.Nome = nome;
            this.Rede = rede;
            this.CEP = cep;
            this.UF = UF;
            this.Endereco = endereco;
            this.Municipio = municipio;
            this.Localizacao = localizacao;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.EtapasEnsino = etapasEnsino;
            this.Total_alunos = totalAlunos;
            this.Situacao = situacao;
            this.Porte = porte;
            this.Telefone = telefone;
            this.Total_docentes = totalDocentes;
        }

        public UsuarioDnit(string nome, string UF, string etapas_ensino, string total_alunos, string situacao, string municipio)
        {
            this.nome = nome;
            this.UF = UF;
            this.etapas_ensino = etapas_ensino;
            this.total_alunos = total_alunos;
            this.situacao = situacao;
            this.municipio = municipio;
        }

    }
}