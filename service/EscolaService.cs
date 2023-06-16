using System.IO;
using AutoMapper;
using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using System.Collections.Generic;

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
            /// arquivo vocês precisam ler o memory stream da planilha e enviar
            /// cada linha pro repositório
            throw new System.NotImplementedException();
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
