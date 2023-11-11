using api.Escolas;
using app.Entidades;

namespace service.Interfaces
{
    public interface ICalcularUpsJob
    {
        public Task ExecutarAsync(PesquisaEscolaFiltro filtro, Ranque novoRanque);
    }
}