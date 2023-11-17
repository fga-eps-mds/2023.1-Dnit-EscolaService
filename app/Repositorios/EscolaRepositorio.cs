﻿using api;
using api.Escolas;
using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace app.Repositorios
{

    public class EscolaRepositorio : IEscolaRepositorio
    {
        private readonly AppDbContext dbContext;

        public EscolaRepositorio(
            AppDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        private IQueryable<Escola> SelecaoEscola(bool incluirEtapas = false, bool incluirMunicipio = false)
        {
            var query = dbContext.Escolas.AsQueryable();

            if (incluirEtapas)
            {
                query = query.Include(e => e.EtapasEnsino);
            }
            if (incluirMunicipio)
            {
                query = query.Include(e => e.Municipio);
            }

            return query;
        }

        public async Task<Escola> ObterPorIdAsync(Guid id, bool incluirEtapas = false, bool incluirMunicipio = false)
        {
            return await SelecaoEscola(incluirEtapas, incluirMunicipio).FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new ApiException(ErrorCodes.EscolaNaoEncontrada);
        }

        public async Task<Escola?> ObterPorCodigoAsync(int codigo, bool incluirEtapas = false, bool incluirMunicipio = false)
        {
            return await SelecaoEscola(incluirEtapas, incluirMunicipio).FirstOrDefaultAsync(e => e.Codigo == codigo);
        }

        public EscolaEtapaEnsino AdicionarEtapaEnsino(Escola escola, EtapaEnsino etapa)
        {
            var model = new EscolaEtapaEnsino
            {
                Id = Guid.NewGuid(),
                Escola = escola,
                EtapaEnsino = etapa,
            };
            dbContext.Add(model);
            return model;
        }

        public Escola Criar(CadastroEscolaData escolaData, Municipio municipio, double distanciaSuperintendencia, Superintendencia? superintendencia)
        {
            var escola = new Escola
            {
                Nome = escolaData.NomeEscola!,
                Codigo = escolaData.CodigoEscola,
                Cep = escolaData.Cep!,
                Endereco = escolaData.Endereco!,
                Latitude = escolaData.Latitude ?? "",
                Longitude = escolaData.Longitude ?? "",
                TotalAlunos = escolaData.NumeroTotalDeAlunos,
                Telefone = escolaData.Telefone ?? "",
                TotalDocentes = escolaData.NumeroTotalDeDocentes,
                Rede = (Rede)escolaData.IdRede,
                Uf = (UF)escolaData.IdUf,
                Localizacao = (Localizacao?)escolaData.IdLocalizacao,
                Porte = (Porte?)escolaData.IdPorte,
                Situacao = (Situacao?)escolaData.IdSituacao,
                DataAtualizacao = DateTimeOffset.Now,
                MunicipioId = municipio.Id,
                Municipio = municipio,
                DistanciaSuperintendencia = distanciaSuperintendencia,
                SuperintendenciaId = superintendencia?.Id,
                Superintendencia = superintendencia,
            };
            dbContext.Add(escola);
            return escola;
        }
        
        public Escola Criar(CadastroEscolaData escolaData, Municipio municipio)
        {
            var escola = new Escola
            {
                Nome = escolaData.NomeEscola!,
                Codigo = escolaData.CodigoEscola,
                Cep = escolaData.Cep!,
                Endereco = escolaData.Endereco!,
                Latitude = escolaData.Latitude ?? "",
                Longitude = escolaData.Longitude ?? "",
                TotalAlunos = escolaData.NumeroTotalDeAlunos,
                Telefone = escolaData.Telefone ?? "",
                TotalDocentes = escolaData.NumeroTotalDeDocentes,
                Rede = (Rede)escolaData.IdRede,
                Uf = (UF)escolaData.IdUf,
                Localizacao = (Localizacao?)escolaData.IdLocalizacao,
                Porte = (Porte?)escolaData.IdPorte,
                Situacao = (Situacao?)escolaData.IdSituacao,
                DataAtualizacao = DateTimeOffset.Now,
                MunicipioId = municipio.Id,
                Municipio = municipio,
            };
            dbContext.Add(escola);
            return escola;
        }

        public Escola Criar(EscolaModel dadosEscola, double distanciaSuperintendencia, Superintendencia? superintendencia)
        {
            var entidade = new Escola()
            {
                Id = Guid.NewGuid(),
                Nome = dadosEscola.NomeEscola,
                Codigo = dadosEscola.CodigoEscola,
                Cep = dadosEscola.Cep,
                Endereco = dadosEscola.Endereco,
                Latitude = dadosEscola.Latitude ?? "",
                Longitude = dadosEscola.Longitude ?? "",
                TotalAlunos = dadosEscola.NumeroTotalDeAlunos ?? 0,
                Telefone = dadosEscola.Telefone,
                TotalDocentes = dadosEscola.NumeroTotalDeDocentes,
                Rede = dadosEscola.Rede!.Value,
                Uf = dadosEscola.Uf,
                Localizacao = dadosEscola.Localizacao,
                MunicipioId = dadosEscola.IdMunicipio,
                Porte = dadosEscola.Porte,
                Situacao = dadosEscola.Situacao,
                Observacao = dadosEscola.Observacao,
                DataAtualizacao = DateTimeOffset.Now,
                DistanciaSuperintendencia = distanciaSuperintendencia,
                Superintendencia = superintendencia,
                SuperintendenciaId = superintendencia?.Id,
            };
            dbContext.Add(entidade);
            return entidade;
        }

        public Escola Criar(EscolaModel dadosEscola)
        {
            var entidade = new Escola()
            {
                Id = Guid.NewGuid(),
                Nome = dadosEscola.NomeEscola,
                Codigo = dadosEscola.CodigoEscola,
                Cep = dadosEscola.Cep,
                Endereco = dadosEscola.Endereco,
                Latitude = dadosEscola.Latitude ?? "",
                Longitude = dadosEscola.Longitude ?? "",
                TotalAlunos = dadosEscola.NumeroTotalDeAlunos ?? 0,
                Telefone = dadosEscola.Telefone,
                TotalDocentes = dadosEscola.NumeroTotalDeDocentes,
                Rede = dadosEscola.Rede!.Value,
                Uf = dadosEscola.Uf,
                Localizacao = dadosEscola.Localizacao,
                MunicipioId = dadosEscola.IdMunicipio,
                Porte = dadosEscola.Porte,
                Situacao = dadosEscola.Situacao,
                Observacao = dadosEscola.Observacao,
                DataAtualizacao = DateTimeOffset.Now,
            };
            dbContext.Add(entidade);
            return entidade;
        }

        public async Task<ListaPaginada<Escola>> ListarPaginadaAsync(PesquisaEscolaFiltro filtro)
        {
            var query = dbContext.Escolas
                .Include(e => e.EtapasEnsino)
                .Include(e => e.Municipio)
                .AsQueryable();

            if (filtro.Nome != null)
            {
                var nome = filtro.Nome.ToLower().Trim();
                query = query.Where(e => e.Nome.ToLower() == nome || e.Nome.ToLower().Contains(nome));
            }
            if (filtro.IdSituacao != null)
            {
                query = query.Where(e => e.Situacao == (Situacao)filtro.IdSituacao);
            }
            if (filtro.IdEtapaEnsino != null)
            {
                var etapas = filtro.IdEtapaEnsino.ConvertAll(e => (EtapaEnsino)e);
                query = query.Where(escola => escola.EtapasEnsino!.Any(etapa => etapas.Contains(etapa.EtapaEnsino)));
            }
            if (filtro.IdMunicipio != null)
            {
                query = query.Where(e => e.MunicipioId == filtro.IdMunicipio);
            }
            if (filtro.IdUf != null)
            {
                query = query.Where(e => e.Uf == (UF)filtro.IdUf);
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.Nome)
                .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina)
                .Take(filtro.TamanhoPagina)
                .ToListAsync();

            return new ListaPaginada<Escola>(items, filtro.Pagina, filtro.TamanhoPagina, total);
        }
    }
}
