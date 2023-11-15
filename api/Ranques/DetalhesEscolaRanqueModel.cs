
namespace api.Escolas
{

    public class DetalhesEscolaRanqueModel
    {
        public RanqueInfo RanqueInfo { get; set; }
        public EscolaModel Escola { get; set; }
    }

    public class RanqueInfo
    {
        public int RanqueId { get; set; }
        public int Pontuacao { get; set; }
        public int Posicao { get; set; }
        public FatorModel[] Fatores { get; set; }
    }

    public class FatorModel
    {
        public string Nome { get; set; }
        public int Peso { get; set; }
        public int Valor { get; set; }
    }
}