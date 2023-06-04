using AutoMapper;
using dominio;
using repositorio;
using service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class SolicitacaoAcaoService : ISolicitacaoAcaoService
    {
        public void CadastraSolicitacaoAcao (SolicitacaoAcaoDTO solicitacaoAcaoDTO)
        {
            var usuario = mapper.Map<UsuarioDnit>(DTO);
           

            usuarioRepositorio.CadastrarUsuarioDnit(usuario);
        }

    }
}
