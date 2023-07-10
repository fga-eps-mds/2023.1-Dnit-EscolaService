using System.IO;
using AutoMapper;
using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using dominio.Dominio;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using System.Diagnostics;

namespace service
{
    public class EscolaService : IEscolaService
    {
        private readonly IEscolaRepositorio escolaRepositorio;
        public EscolaService(IEscolaRepositorio escolaRepositorio)
        {
            this.escolaRepositorio = escolaRepositorio;
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

        public List<string> CadastrarEscolaViaPlanilha(MemoryStream planilha)
        {
            List<string> escolasNovas = new List<string>();
            using (var reader = new StreamReader(planilha))
            {
                using (var parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");

                    bool primeiralinha = false;

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

                            Escola escola = new Escola();
                            escola.CodigoEscola = int.Parse(linha[colunas["codigo_inep"]]);
                            escola.NomeEscola = linha[colunas["nome_escola"]];
                            escola.IdRede = ObterRedePeloId(linha[colunas["rede"]]);
                            escola.IdPorte = ObterPortePeloId(linha[colunas["porte"]]);
                            escola.Endereco = linha[colunas["endereco"]];
                            escola.Cep = linha[colunas["cep"]];
                            escola.IdUf = ObterEstadoPelaSigla(linha[colunas["uf"]]);
                            escola.IdLocalizacao = ObterLocalizacaoPeloId(linha[colunas["localizacao"]]);
                            escola.Latitude = linha[colunas["latitude"]];
                            escola.Longitude = linha[colunas["longitude"]];
                            escola.Telefone = linha[colunas["ddd"]] + linha[colunas["telefone"]];
                            List<int> etapas_lista = EtapasParaIds(linha[colunas["etapas_ensino"]], escola.NomeEscola);
                            escola.NumeroTotalDeAlunos = 0;
                            for (int i = colunas["qtd_ensino_infantil"]; i <= colunas["qtd_ensino_fund_9ano"]; i++)
                            {
                                string alunos = linha[i];
                                int quantidade;
                                if (int.TryParse(alunos, out quantidade)) escola.NumeroTotalDeAlunos += quantidade;
                            }
                            escola.NumeroTotalDeDocentes = int.Parse(linha[colunas["qtd_docentes"]]);

                            //Lançando exceções para erro nas colunas da planilha inserida

                            string municipio = ObterCodigoMunicipioPorCEP(escola.Cep).GetAwaiter().GetResult();
                            int codigoMunicipio;
                            if (int.TryParse(municipio, out codigoMunicipio))
                            {
                                escola.IdMunicipio = int.Parse(municipio);
                            }
                            else
                            {
                                throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", CEP inválido!");
                            }

                            if (escola.IdRede == 0)
                            {
                                throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", rede inválida!");
                            }

                            if (escola.IdUf == 0)
                            {
                                throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", UF inválida!");
                            }

                            if (escola.IdLocalizacao == 0)
                            {
                                throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", localização inválida!");
                            }

                            if (escola.IdPorte == 0)
                            {
                                throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", descrição do porte inválida!");
                            }


                            //Atualizando ou inserindo escolas no banco de dados

                            if (escolaRepositorio.EscolaJaExiste(escola.CodigoEscola))
                            {
                                escolaRepositorio.AtualizarDadosPlanilha(escola);
                                continue;
                            }

                            escolasNovas.Add(escola.NomeEscola);

                            int id = escolaRepositorio.CadastrarEscola(escola);

                            foreach (var id_etapa in etapas_lista)
                            {
                                escolaRepositorio.CadastrarEtapasDeEnsino(id, id_etapa);
                            }
                        }
                        catch (FormatException ex)
                        {
                            throw new Exception("Planilha com formato incompatível.");
                        }
                    }
                }
            }
            return escolasNovas;
        }

        public void ExcluirEscola(int id)
        {
            escolaRepositorio.ExcluirEscola(id);

        }

        public void CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO)
        {
            cadastroEscolaDTO.UltimaAtualizacao = DateTime.Now;

            int idEscola = escolaRepositorio.CadastrarEscola(cadastroEscolaDTO) ?? 0;

            if (idEscola == 0)
            {
                throw new Exception("Erro ao realizar cadastro de escola");
            }

            foreach (int idSituacao in cadastroEscolaDTO.IdEtapasDeEnsino)
            {
                escolaRepositorio.CadastrarEtapasDeEnsino(idEscola, idSituacao);
            }
        }

        public void RemoverSituacaoEscola(int idEscola)
        {
            escolaRepositorio.RemoverSituacaoEscola(idEscola);
        }

        public ListaPaginada<EscolaCorreta> Obter(PesquisaEscolaFiltro pesquisaEscolaFiltro)
        {
            return escolaRepositorio.ObterEscolas(pesquisaEscolaFiltro);
        }

        public async Task<string> ObterCodigoMunicipioPorCEP(string cep)
        {
            string url = $"https://viacep.com.br/ws/{cep}/json/";

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
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public int ObterEstadoPelaSigla(string UF)
        {
            Dictionary<int, string> estados = new Dictionary<int, string>()
            {
                { 1, "AC"},
                { 2, "AL"},
                { 3, "AP"},
                { 4, "AM"},
                { 5, "BA"},
                { 6, "CE"},
                { 7, "ES"},
                { 8, "GO"},
                { 9, "MA"},
                { 10, "MT"},
                { 11, "MS"},
                { 12, "MG"},
                { 13, "PA"},
                { 14, "PB"},
                { 15, "PR"},
                { 16, "PE"},
                { 17, "PI"},
                { 18, "RJ"},
                { 19, "RN"},
                { 20, "RS"},
                { 21, "RO"},
                { 22, "RR"},
                { 23, "SC"},
                { 24, "SP"},
                { 25, "SE"},
                { 26, "TO"},
                { 27, "DF"}
            };

            foreach (var estado in estados)
            {
                if (estado.Value == UF.ToUpper()) return estado.Key;
            }
            return 0;
        }

        public int ObterPortePeloId(string Porte)
        {
            Dictionary<int, string> porte = new Dictionary<int, string>()
            {
                { 1, "Até 50 matrículas de escolarização"},
                { 2, "Entre 201 e 500 matrículas de escolarização"},
                { 3, "Entre 501 e 1000 matrículas de escolarização"},
                { 4, "Entre 51 e 200 matrículas de escolarização"},
                { 5, "Mais de 1000 matrículas de escolarização"},
            };

            foreach (var descricao in porte)
            {
                if (descricao.Value == Porte) return descricao.Key;
            }
            return 0;
        }

        public List<int> EtapasParaIds(string etapas, string nomeEscola)
        {
            List<int> ids = new List<int>();

            List<string> etapas_separadas = etapas.Split(',').Select(item => item.Trim()).ToList();

            Dictionary<int, string> descricao_etapas = new Dictionary<int, string>()
            {
                { 1, "Educação Infantil"},
                { 2, "Ensino Fundamental"},
                { 3, "Ensino Médio"},
                { 4, "Educação de Jovens Adultos"},
                { 5, "Educação Profissional"},
            };

            foreach (var nome in etapas_separadas)
            {
                foreach (var etapa in descricao_etapas)
                {
                    if (etapa.Value.ToLower() == nome.ToLower())
                    {
                        ids.Add(etapa.Key);
                        break;
                    }
                }
            }
            if (ids.Count == 0 || ids.Count != etapas_separadas.Count)
            {
                throw new Exception("Erro. A leitura do arquivo parou na escola: " + nomeEscola + ", descrição das etapas de ensino inválida!");
            }
            return ids;
        }

        public int ObterRedePeloId(string Rede)
        {
            Dictionary<int, string> redes = new Dictionary<int, string>()
            {
                { 1, "Municipal"},
                { 2, "Estadual"},
                { 3, "Privada"},
            };

            foreach (var descricao in redes)
            {
                if (descricao.Value.ToLower() == Rede.ToLower()) return descricao.Key;
            }
            return 0;
        }

        public int ObterLocalizacaoPeloId(string Localizacao)
        {
            if (Localizacao.ToLower() == "rural")
            {
                return 1;
            }
            else if (Localizacao.ToLower() == "urbana")
            {
                return 2;
            }
            return 0;
        }
        public void AlterarDadosEscola(AtualizarDadosEscolaDTO atualizarDadosEscolaDTO)
        {
            if (atualizarDadosEscolaDTO.IdSituacao == 0) atualizarDadosEscolaDTO.IdSituacao = null;
            atualizarDadosEscolaDTO.UltimaAtualizacao = DateTime.Now;
            escolaRepositorio.AlterarDadosEscola(atualizarDadosEscolaDTO);
        }
    }
}


