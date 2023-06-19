using System.IO;
using AutoMapper;
using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace service
{
    public class EscolaService : IEscolaService
    {
        private readonly IEscolaRepositorio escolaRepositorio;
        public EscolaService(IEscolaRepositorio escolaRepositorio)
        {
            this.escolaRepositorio = escolaRepositorio;
        }

        public void CadastrarEscola(Escola escola)
        {
            escolaRepositorio.CadastrarEscola(escola);
        }

        public void CadastrarEscolaViaPlanilha(MemoryStream planilha)
        {
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
                    }
                }
            }
        }
        public IEnumerable<Escola> Listar()
        {
            return escolaRepositorio.Obter();
        }

        public Escola Listar(int idEscola)
        {
            Escola escola = escolaRepositorio.Obter(idEscola);
            
            return escola;
        }

        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO)
        {
            escolaRepositorio.AdicionarSituacao(atualizarSituacaoDTO.IdSituacao, atualizarSituacaoDTO.IdEscola);
        }
    }
}
