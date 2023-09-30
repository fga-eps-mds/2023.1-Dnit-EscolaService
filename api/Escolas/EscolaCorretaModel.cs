namespace api.Escolas
{
    public class EscolaCorretaModel : EscolaModel
    {
        public Dictionary<int, string>? EtapaEnsino { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
    }
}
