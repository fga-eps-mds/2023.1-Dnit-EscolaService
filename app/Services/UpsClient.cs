using System.Text.Json;
using service.Interfaces;

namespace app.Services
{
    public class UpsClient : IUpsClient
    {
        public async Task<IEnumerable<int>> CalcularUps(IEnumerable<LocalizacaoEscola> localizacoes)
        {
            var client = new HttpClient { };
            var raio = 2.0D;
            var desde = 2019;

            // https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
            // FIXME: converte número double em string
            var conteudo = JsonContent.Create(localizacoes);
            var resposta = await client.PostAsync(
                // $"http://localhost:7085/api/calcular/ups/escolas?desde={desde}&raiokm={raio}",
                $"http://localhost:7085/api/calcular/ups/escolas",
                conteudo);

            resposta.EnsureSuccessStatusCode();

            var upss = await resposta.Content.ReadFromJsonAsync<IEnumerable<int>>();
            Console.WriteLine($"UPSs: {upss}\n");
            return upss!;
        }
    }
}