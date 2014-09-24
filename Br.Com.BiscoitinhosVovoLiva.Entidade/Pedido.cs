using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.Com.BiscoitinhosVovoLiva.Entidade
{
    [Serializable]
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string Email { get; set; }
        public int Qtdade { get; set; }
    }
}
