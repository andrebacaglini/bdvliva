using System.Collections.Generic;
using Br.Com.BiscoitinhosVovoLiva.Entidade;

namespace Br.Com.BiscoitinhosVovoLiva.Repositorio
{
    public interface IPedidoRepositorio
    {
        void Salvar(Pedido pedido);        

        List<Pedido> Listar();
    }
}
