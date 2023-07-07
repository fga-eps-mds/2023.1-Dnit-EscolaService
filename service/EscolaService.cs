using System.IO;
using AutoMapper;
using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        public List<int> CadastrarEscolaViaPlanilha(MemoryStream planilha)
        {
            List<int> escolasDuplicadas = new List<int>();
            int numero_linha = 2;
            using (var reader = new StreamReader(planilha))
            {
                using (var parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");

                    bool primeiralinha = false;

                    while (!parser.EndOfData)
                    {
                        string[] linha = parser.ReadFields();
                        if (!primeiralinha)
                        {
                            primeiralinha = true;
                            continue;
                        }
                        Escola escola = new Escola();
                        escola.NomeEscola = linha[0];
                        escola.CodigoEscola = int.Parse(linha[1]);
                        if (escolaRepositorio.EscolaJaExiste(escola.CodigoEscola))
                        {
                            escolasDuplicadas.Add(numero_linha);
                            numero_linha++;
                            continue;
                        }
                        escola.Cep = linha[2];
                        escola.Endereco = linha[3];
                        escola.Latitude = linha[4];
                        escola.Longitude = linha[5];
                        escola.NumeroTotalDeAlunos = int.Parse(linha[6]);
                        escola.Telefone = linha[7];
                        escola.NumeroTotalDeDocentes = int.Parse(linha[8]);
                        escola.IdEscola = int.Parse(linha[9]);
                        escola.IdRede = int.Parse(linha[10]);
                        escola.IdUf = int.Parse(linha[11]);
                        escola.IdLocalizacao = int.Parse(linha[12]);
                        escola.IdMunicipio = int.Parse(linha[13]);
                        escola.IdEtapasDeEnsino = int.Parse(linha[14]);
                        escola.IdPorte = int.Parse(linha[15]);
                        escola.IdSituacao = int.Parse(linha[16]);

                        escolaRepositorio.CadastrarEscola(escola);
                        numero_linha++;
                    }
                }
            }
            return escolasDuplicadas;
        }

        public void ExcluirEscola(int id)
        {
            escolaRepositorio.ExcluirEscola(id);

        }

        public void CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO)
        {
            int idEscola = escolaRepositorio.CadastrarEscola(cadastroEscolaDTO) ?? 0;

            if(idEscola == 0)
            {
                throw new Exception("Erro ao realizar cadastro de escola");
            }

            foreach(int idSituacao in cadastroEscolaDTO.IdEtapasDeEnsino)
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

        public void AlterarDadosEscola(AtualizarDadosEscolaDTO atualizarDadosEscolaDTO)
        {
            if (atualizarDadosEscolaDTO.IdSituacao == 0) atualizarDadosEscolaDTO.IdSituacao = null;
            atualizarDadosEscolaDTO.UltimaAtualizacao = DateTime.Now;
            escolaRepositorio.AlterarDadosEscola(atualizarDadosEscolaDTO);

            //escolaRepositorio.RemoverEtapasDeEnsino(atualizarDadosEscolaDTO.IdEscola);

            // foreach(int idEtapaEnsino in atualizarDadosEscolaDTO.IdEtapasDeEnsino)
            // {
            //     escolaRepositorio.CadastrarEtapasDeEnsino(atualizarDadosEscolaDTO.IdEscola, idEtapaEnsino);
            // }
        }
    }
}


