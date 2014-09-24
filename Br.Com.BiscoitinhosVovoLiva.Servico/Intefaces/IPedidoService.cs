using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Br.Com.BiscoitinhosVovoLiva.Entidade;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces
{
    public interface IPedidoService
    {
        void Salvar(Pedido pedido);
    }
}
