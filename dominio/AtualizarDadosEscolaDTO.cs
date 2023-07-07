namespace dominio
{
    public class AtualizarDadosEscolaDTO
    {
        public int IdEscola {get; set;}
        public int? IdSituacao {get; set;}
        public string Telefone {get; set;}
        public string Longitude {get; set;}
        public string Latitude {get; set;}
        public int NumeroTotalDeAlunos {get; set;}
        public int NumeroTotalDeDocentes {get; set;}
        public string Observacao {get; set;}
        public DateTime UltimaAtualizacao{get;set;}
    }
}
