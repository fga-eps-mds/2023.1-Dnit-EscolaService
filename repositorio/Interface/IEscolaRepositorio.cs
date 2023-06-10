using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IEscolaRepositorio
    {
        public Escola ListarInformacoesEscolas(int id_escola);
        public void AdicionarSituacao(int id_situacao, int id_escola);
    }
}
