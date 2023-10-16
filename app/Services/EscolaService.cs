using service.Interfaces;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using app.Entidades;
using EnumsNET;
using api;
using app.Repositorios.Interfaces;
using api.Escolas;

namespace app.Services
{
    public class EscolaService : IEscolaService
    {
        private readonly IEscolaRepositorio escolaRepositorio;
        private readonly IMunicipioRepositorio municipioRepositorio;
        private readonly ModelConverter modelConverter;
        private readonly AppDbContext dbContext;

        public EscolaService(
            IEscolaRepositorio escolaRepositorio,
            IMunicipioRepositorio municipioRepositorio,
            ModelConverter modelConverter,
            AppDbContext dbContext
        )
        {
            this.escolaRepositorio = escolaRepositorio;
            this.municipioRepositorio = municipioRepositorio;
            this.modelConverter = modelConverter;
            this.dbContext = dbContext;
        }

        public bool SuperaTamanhoMaximo(MemoryStream planilha)
        {
            using (var reader = new StreamReader(planilha))
            {
                int tamanho_max = 5000;
                int quantidade_escolas = -1;

                while (reader.ReadLine() != null) { quantidade_escolas++; }

                return quantidade_escolas > tamanho_max;
            }
        }

        public async Task CadastrarAsync(CadastroEscolaDTO escolaData)
        {
            var municipioId = escolaData.IdMunicipio ?? throw new ApiException(ErrorCodes.MunicipioNaoEncontrado);
            var municipio = await municipioRepositorio.ObterPorIdAsync(municipioId);
            var escola = escolaRepositorio.Criar(escolaData, municipio);
            escolaData.IdEtapasDeEnsino
                ?.Select(e => (EtapaEnsino)e)
                ?.Select(e => escolaRepositorio.AdicionarEtapaEnsino(escola, e))
                ?.ToList();

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<string>> CadastrarAsync(MemoryStream planilha)
        {
            var escolasNovas = new List<string>();
            var redes = Enum.GetValues<Rede>().ToDictionary(r => r.ToString().ToLower());
            var localizacoes = Enum.GetValues<Localizacao>().ToDictionary(l => l.ToString().ToLower());
            var ufs = Enum.GetValues<UF>().ToDictionary(uf => uf.ToString().ToLower());
            var portes = Enum.GetValues<Porte>()
                .ToDictionary(p => p.AsString(EnumFormat.Description)?.ToLower()
                                    ?? throw new Exception($"Enum ${nameof(Porte)} deve ter descrição"));
            var etapas = Enum.GetValues<EtapaEnsino>()
                .ToDictionary(e => e.AsString(EnumFormat.Description)?.ToLower()
                                    ?? throw new Exception($"Enum {nameof(EtapaEnsino)} deve ter descrição"));

            using (var reader = new StreamReader(planilha))
            using (var parser = new TextFieldParser(reader))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                var primeiralinha = false;

                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] linha = parser.ReadFields();
                        if (!primeiralinha)
                        {
                            primeiralinha = true;
                            continue;
                        }

                        Dictionary<string, int> colunas = new Dictionary<string, int>
                        {
                            {"ano_senso", 0 },
                            {"id", 1 },
                            {"codigo_inep", 2 },
                            {"nome_escola", 3 },
                            {"rede", 4 },
                            {"porte", 5 },
                            {"endereco", 6 },
                            {"cep", 7 },
                            {"cidade", 8 },
                            {"uf", 9 },
                            {"localizacao", 10 },
                            {"latitude", 11 },
                            {"longitude", 12 },
                            {"ddd", 13 },
                            {"telefone", 14 },
                            {"etapas_ensino", 15 },
                            {"qtd_ensino_infantil", 16 },
                            {"qtd_ensino_fund_1ano", 17 },
                            {"qtd_ensino_fund_2ano", 18 },
                            {"qtd_ensino_fund_3ano", 19 },
                            {"qtd_ensino_fund_4ano", 20 },
                            {"qtd_ensino_fund_5ano", 21 },
                            {"qtd_ensino_fund_6ano", 22 },
                            {"qtd_ensino_fund_7ano", 23 },
                            {"qtd_ensino_fund_8ano", 24 },
                            {"qtd_ensino_fund_9ano", 25 },
                            {"qtd_docentes", 26 },
                        };

                        var escola = new EscolaModel()
                        {
                            CodigoEscola = int.Parse(linha[colunas["codigo_inep"]]),
                            NomeEscola = linha[colunas["nome_escola"]],
                            Uf = ufs.GetValueOrDefault(linha[colunas["uf"]].ToLower()),
                            Rede = redes.GetValueOrDefault(linha[colunas["rede"]].ToLower()),
                            Porte = portes.GetValueOrDefault(linha[colunas["porte"]].ToLower()),
                            Localizacao = localizacoes.GetValueOrDefault(linha[colunas["localizacao"]].ToLower()),
                            Endereco = linha[colunas["endereco"]],
                            Cep = linha[colunas["cep"]],
                            Latitude = linha[colunas["latitude"]],
                            Longitude = linha[colunas["longitude"]],
                            Telefone = linha[colunas["ddd"]] + linha[colunas["telefone"]],
                            NumeroTotalDeDocentes = int.Parse(linha[colunas["qtd_docentes"]]),
                            EtapasEnsino = etapasParaIds(etapas, linha[colunas["etapas_ensino"]]),
                        };

                        for (int i = colunas["qtd_ensino_infantil"]; i <= colunas["qtd_ensino_fund_9ano"]; i++)
                        {
                            var alunos = linha[i];
                            int quantidade;
                            if (int.TryParse(linha[i], out quantidade)) escola.NumeroTotalDeAlunos += quantidade;
                        }

                        //Lançando exceções para erro nas colunas da planilha inserida

                        var municipio = await ObterCodigoMunicipioPorCEPAsync(escola.Cep);
                        int codigoMunicipio;
                        if (int.TryParse(municipio, out codigoMunicipio))
                        {
                            escola.IdMunicipio = int.Parse(municipio);
                        }
                        else
                        {
                            throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", CEP inválido!");
                        }

                        if (escola.EtapasEnsino.Count == 0 || escola.EtapasEnsino.Count != linha[colunas["etapas_ensino"]].Split(",").Count())
                        {
                            throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", descrição das etapas de ensino inválida!");
                        }

                        if (escola.Rede == default(Rede))
                        {
                            throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", rede inválida!");
                        }

                        if (escola.Uf == default(UF))
                        {
                            throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", UF inválida!");
                        }

                        if (escola.Localizacao == default(Localizacao))
                        {
                            throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", localização inválida!");
                        }

                        if (escola.Porte == default(Porte))
                        {
                            throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", descrição do porte inválida!");
                        }


                        //Atualizando ou inserindo escolas no banco de dados
                        var escolaExistente = await escolaRepositorio.ObterPorCodigoAsync(escola.CodigoEscola);
                        if (escolaExistente != default)
                        {
                            await AtualizarAsync(escolaExistente, escola);
                            continue;
                        }

                        var escolaNova = escolaRepositorio.Criar(escola);
                        foreach (var etapa in escola.EtapasEnsino)
                        {
                            escolaRepositorio.AdicionarEtapaEnsino(escolaNova, etapa);
                        }

                        await dbContext.SaveChangesAsync();

                        escolasNovas.Add(escolaNova.Nome);
                    }
                    catch (FormatException)
                    {
                        throw new Exception("Planilha com formato incompatível.");
                    }
                }
            }
            return escolasNovas;
        }

        public async Task AtualizarAsync(Escola escola, EscolaModel data, List<EtapaEnsino>? etapasData = null)
        {
            escola.Nome = data.NomeEscola;
            escola.Codigo = data.CodigoEscola;
            escola.Cep = data.Cep;
            escola.Endereco = data.Endereco;
            escola.Latitude = data.Latitude;
            escola.Longitude = data.Longitude;
            escola.TotalAlunos = data.NumeroTotalDeAlunos ?? 0;
            escola.TotalDocentes = data.NumeroTotalDeDocentes;
            escola.Telefone = data.Telefone;
            escola.Rede = data.Rede.Value;
            escola.Uf = data.Uf;
            escola.Localizacao = data.Localizacao;
            escola.MunicipioId = data.IdMunicipio;
            escola.Porte = data.Porte;
            escola.DataAtualizacao = DateTimeOffset.Now;

            atualizarEtapasEnsino(escola, data.EtapasEnsino);
            await dbContext.SaveChangesAsync();
        }

        private void atualizarEtapasEnsino(Escola escola, List<EtapaEnsino> etapas)
        {
            var etapasExistentes = escola.EtapasEnsino.Select(e => e.EtapaEnsino).ToList();
            var etapasDeletadas = escola.EtapasEnsino.Where(e => !etapas.Contains(e.EtapaEnsino));
            var etapasNovas = etapas.Where(e => !etapasExistentes.Contains(e));

            foreach (var etapa in etapasDeletadas)
            {
                dbContext.Remove(etapa);
            }
            foreach (var etapa in etapasNovas)
            {
                escolaRepositorio.AdicionarEtapaEnsino(escola, etapa);
            }
        }

        public async Task ExcluirAsync(Guid id)
        {
            var escola = await escolaRepositorio.ObterPorIdAsync(id);
            dbContext.Remove(escola);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoverSituacaoAsync(Guid id)
        {
            var escola = await escolaRepositorio.ObterPorIdAsync(id);
            escola.Situacao = null;
            await dbContext.SaveChangesAsync();
        }

        public async Task<ListaEscolaPaginada<EscolaCorretaModel>> ListarPaginadaAsync(PesquisaEscolaFiltro filtro)
        {
            var escolas = await escolaRepositorio.ListarPaginadaAsync(filtro);
            var escolasCorretas = escolas.Items.ConvertAll(modelConverter.ToModel);
            return new ListaEscolaPaginada<EscolaCorretaModel>(escolasCorretas, escolas.Pagina, escolas.ItemsPorPagina, escolas.Total);
        }

        public async Task<string?> ObterCodigoMunicipioPorCEPAsync(string cep)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json/";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var conteudo = await response.Content.ReadAsStringAsync();
                        dynamic resultado = JObject.Parse(conteudo);

                        return resultado.ibge;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private List<EtapaEnsino> etapasParaIds(Dictionary<string, EtapaEnsino> etapas, string coluna_etapas)
        {
            var ids = new List<int>();
            var etapas_separadas = coluna_etapas.Split(',').Select(item => item.Trim());
            var resultado = etapas_separadas.Select(e => etapas.GetValueOrDefault(e.ToLower())).Where(e => e != default).ToList();
            return resultado;
        }

        public async Task AlterarDadosEscolaAsync(AtualizarDadosEscolaDTO dados)
        {
            var escola = await escolaRepositorio.ObterPorIdAsync(dados.IdEscola, incluirEtapas: true);

            escola.DataAtualizacao = DateTime.Now;
            escola.Telefone = dados.Telefone;
            escola.Longitude = dados.Longitude;
            escola.Latitude = dados.Latitude;
            escola.TotalAlunos = dados.NumeroTotalDeAlunos;
            escola.TotalDocentes = dados.NumeroTotalDeDocentes;
            escola.Observacao = dados.Observacao;
            escola.Situacao = (Situacao?)dados.IdSituacao;

            atualizarEtapasEnsino(escola, dados.IdEtapasDeEnsino.ConvertAll(e => (EtapaEnsino)e));

            await dbContext.SaveChangesAsync();
        }
    }
}


