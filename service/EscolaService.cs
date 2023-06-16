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

                    while (!parser.EndOfData)
                    {
                        string[] linha = parser.ReadFields();
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
