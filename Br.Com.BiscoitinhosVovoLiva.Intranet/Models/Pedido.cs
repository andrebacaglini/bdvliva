using System;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet.Models
{
    [Serializable]
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string Email { get; set; }
        public int Qtdade { get; set; }
    }
}