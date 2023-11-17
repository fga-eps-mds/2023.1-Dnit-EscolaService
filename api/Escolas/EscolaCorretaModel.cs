namespace api.Escolas
{
    public class EscolaCorretaModel : EscolaModel
    {
        public double DistanciaSuperintendencia { get; set; }
        public int? SuperintendenciaId { get; set; }
        public Dictionary<int, string>? EtapaEnsino { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
    }
}
