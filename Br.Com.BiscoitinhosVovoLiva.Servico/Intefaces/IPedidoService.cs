using System.Collections.Generic;
using Br.Com.BiscoitinhosVovoLiva.Entidade;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces
{
    public interface IPedidoService
    {
        /// <summary>
        /// Salva o pedido solicitado
        /// </summary>
        /// <param name="pedido"></param>
        void Salvar(Pedido pedido);

        /// <summary>
        /// Atualiza os dados mutaveis (quantidade e status de pagamento) do pedido.
        /// </summary>
        /// <param name="pedido"></param>
        void Atualizar(Pedido pedido);

        /// <summary>
        /// Lista todos os pedidos cadastrados.
        /// </summary>
        /// <returns></returns>
        List<Pedido> Listar();

        /// <summary>
        /// Consulta um pedido especifico pelo identificador (Login).
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Pedido ConsultarPorLogin(string login);
    }
}
