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
        public string Login { get; set; }
        public string Email { get; set; }
        public int Qtdade { get; set; }
        public bool Pegou { get; set; }
        public bool Pagou { get; set; }
    }
}
