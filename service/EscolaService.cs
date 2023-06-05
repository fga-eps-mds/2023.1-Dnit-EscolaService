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

        public void ListaEscolas(Escola escola)
        {
            escolaRepositorio.ListarEscolas(escola.Nome);
        }
    }
}