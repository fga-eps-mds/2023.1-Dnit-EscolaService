namespace dominio
{
    public class ListaPaginada<T>
    {
        public int Pagina { get; set; }
        public int EscolasPorPagina { get; set; }
        public int TotalEscolas { get; set; }
        public int TotalPaginas { get; set; }
        public List<T> Escolas { get; set; }


        public ListaPaginada(IEnumerable<T> escolas, int paginaIndex, int escolasPorPagina, int totalEscolas)
        {
            Pagina = paginaIndex;
            EscolasPorPagina = escolasPorPagina;
            TotalEscolas = totalEscolas;
            TotalPaginas = (int)Math.Ceiling(TotalPaginas / (double)EscolasPorPagina);
            Escolas = new List<T>(escolas);
        }
    }
}
