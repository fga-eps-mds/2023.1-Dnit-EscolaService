using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public Escola ListarInformacoesEscolas(string nome);
        public void AdicionarSituacao(string situacao);
    }
}
