using api.Escolas;

namespace service.Interfaces
{
    public interface ICalcularUpsJob
    {
        public Task ExecutarAsync(PesquisaEscolaFiltro filtro, int novoRanqueId);
        public Task FinalizarCalcularUpsJob(int ranqueId);
    }
}