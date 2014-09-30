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
    public class PedidoService : BaseService, IPedidoService
    {
        #region Repositorio

        public IPedidoRepositorio PedidoRepositorio { get; set; }

        #endregion

        public void Salvar(Pedido pedido)
        {
            try
            {
                // TODO: Verificar se email já possui pedido para semana - Exceção caso já tenha pedido

                PedidoRepositorio.Salvar(pedido);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pedido> Listar()
        {
            try
            {
                return PedidoRepositorio.Listar();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
