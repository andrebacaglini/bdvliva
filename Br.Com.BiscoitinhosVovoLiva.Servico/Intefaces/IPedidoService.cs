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
        /// Atualiza os dados mutaveis (quantidade e status de pagamento) do pedido atual
        /// </summary>
        /// <param name="pedido"></param>
        void Atualizar(Pedido pedido);

        /// <summary>
        /// Atualiza os dados mutaveis (quantidade e status de pagamento) do pedido da semana desejada.
        /// </summary>
        /// <param name="pedido"></param>
        /// <param name="numeroSemana"></param>
        void Atualizar(Pedido pedido, int numeroSemana);

        /// <summary>
        /// Lista todos os pedidos cadastrados.
        /// </summary>
        /// <returns></returns>
        List<Pedido> Listar();

        /// <summary>
        /// Lista todos os pedidos cadastrados de uma determinada semana.
        /// </summary>
        /// <param name="numeroSemana"></param>
        /// <returns></returns>
        List<Pedido> Listar(int numeroSemana);

        /// <summary>
        /// Consulta um pedido especifico da semana atual pelo login.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Pedido ConsultarPedido(string login);

        /// <summary>
        /// Consulta um pedido especifico da semana desejada pelo login.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="numeroSemana"></param>
        /// <returns></returns>
        Pedido ConsultarPedido(string login, int numeroSemana);

        
    }
}
