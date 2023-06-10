using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System;
using BCryptNet = BCrypt.Net.BCrypt;

namespace service
{
    public class EscolaService : IEscolaService
    {

        private readonly IEscolaRepositorio escolaRepositorio;
        private readonly IMapper mapper;

        public EscolaService(IEscolaRepositorio escolaRepositorio, IMapper mapper)
        {
            this.escolaRepositorio = escolaRepositorio;
            this.mapper = mapper;
        }

        public Escola ListarInformacoesEscolas(int id_escola)
        {
            Escola escola = escolaRepositorio.ListarInformacoesEscolas(id_escola);
            
            return escola;
        }

        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO)
        {
            escolaRepositorio.AdicionarSituacao(atualizarSituacaoDTO.id_situacao, atualizarSituacaoDTO.id_escola);
        }

    }
}