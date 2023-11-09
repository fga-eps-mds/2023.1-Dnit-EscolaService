using app.Services;

namespace service.Interfaces
{
    public interface IUpsClient
    {
        public Task<IEnumerable<int>> CalcularUps(IEnumerable<LocalizacaoEscola> localizacoes);
    }
}