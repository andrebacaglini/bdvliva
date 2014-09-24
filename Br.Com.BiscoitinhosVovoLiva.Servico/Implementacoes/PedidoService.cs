using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using Br.Com.BiscoitinhosVovoLiva.Repositorio;
using Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.Implementacoes
{
    public class PedidoService : IPedidoService
    {
        #region Repositorio

        public IPedidoRepositorio PedidoRepositorio { get; set; }

        #endregion

        public void Salvar(Pedido pedido)
        {
            try
            {
                PedidoRepositorio.Salvar(pedido);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
