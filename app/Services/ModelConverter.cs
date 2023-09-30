using api;
using app.Entidades;
using EnumsNET;

namespace app.Services
{
    public class ModelConverter
    {
        public EscolaCorretaModel ToModel(Escola value) =>
            new EscolaCorretaModel
            {
                IdEscola = value.Id,
                CodigoEscola = value.Codigo,
                NomeEscola = value.Nome,
                Telefone = value.Telefone,
                UltimaAtualizacao = value.AtualizacaoDate?.LocalDateTime,
                Cep = value.Cep,
                Endereco = value.Endereco,
                Uf = value.Uf,
                IdUf = (int?)value.Uf,
                SiglaUf = value.Uf?.ToString(),
                DescricaoUf = value.Uf?.AsString(EnumFormat.Description),
                IdSituacao = (int?)value.Situacao,
                Situacao = value.Situacao,
                DescricaoSituacao = value.Situacao?.AsString(EnumFormat.Description),
                IdRede = (int?)value.Rede,
                Rede = value.Rede,
                DescricaoRede = value.Rede.AsString(EnumFormat.Description),
                IdPorte = (int?)value.Porte,
                Porte = value.Porte,
                Observacao = value.Observacao,
                IdLocalizacao = (int?)value.Localizacao,
                Localizacao = value.Localizacao,
                DescricaoLocalizacao = value.Localizacao?.AsString(EnumFormat.Description),
                Latitude = value.Latitude,
                Longitude = value.Longitude,
                NumeroTotalDeDocentes = value.TotalDocentes,
                NumeroTotalDeAlunos = value.TotalAlunos,
                IdMunicipio = value.MunicipioId,

                NomeMunicipio = value.Municipio?.Nome,
                EtapasEnsino = value.EtapasEnsino.ConvertAll(e => e.EtapaEnsino),
                EtapaEnsino = value.EtapasEnsino.ToDictionary(e => (int)e.EtapaEnsino, e => e.EtapaEnsino.AsString(EnumFormat.Description) ?? ""),
            };
    }
}
