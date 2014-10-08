using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using Br.Com.BiscoitinhosVovoLiva.Repositorio;
using Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.Implementacoes
{
    public class PedidoService : BaseService, IPedidoService
    {
        #region Servicos

        public IUsuarioService ServicoUsuario { get; set; }

        #endregion

        #region Repositorios

        public IPedidoRepositorio PedidoRepositorio { get; set; }

        #endregion

        /// <summary>
        /// Salva o pedido solicitado
        /// </summary>
        /// <param name="pedido"></param>
        public void Salvar(Pedido pedido)
        {
            ValidaLoginUsuario(pedido.Login);
            ValidaPedidoExistente(pedido.Login);
            ValidaQtdadePaes(pedido.Qtdade);
            PedidoRepositorio.Salvar(pedido);
        }

        /// <summary>
        /// Atualiza os dados mutaveis (quantidade e status de pagamento) do pedido atual
        /// </summary>
        /// <param name="pedido"></param>
        public void Atualizar(Pedido pedidoNovo)
        {
            ValidaLoginUsuario(pedidoNovo.Login);
            ValidaQtdadePaes(pedidoNovo.Qtdade);
            var todosPedidos = Listar();

            var pedidoExistente = todosPedidos.FirstOrDefault(x => string.Equals(x.Login, pedidoNovo.Login));
            if (pedidoExistente == null)
            {
                throw new ApplicationException("ERRO_PEDIDO_NAO_ENCONTRADO");
            }
            else
            {
                pedidoExistente.Qtdade = pedidoNovo.Qtdade;
                pedidoExistente.Pagou = pedidoNovo.Pagou;
                pedidoExistente.Pegou = pedidoNovo.Pegou;
            }

            PedidoRepositorio.Atualizar(todosPedidos);
        }

        /// <summary>
        /// Atualiza os dados mutaveis (quantidade e status de pagamento) do pedido da semana desejada.
        /// </summary>
        /// <param name="pedidoNovo"></param>
        /// <param name="numeroSemana"></param>
        public void Atualizar(Pedido pedidoNovo, int numeroSemana)
        {
            ValidaLoginUsuario(pedidoNovo.Login);
            ValidaQtdadePaes(pedidoNovo.Qtdade);

            var todosPedidos = new List<Pedido>();
            if (numeroSemana == 0)
            {
                todosPedidos = Listar();
            }
            else
            {
                todosPedidos = Listar(numeroSemana);
            }


            var pedidoExistente = todosPedidos.FirstOrDefault(x => string.Equals(x.Login, pedidoNovo.Login));
            if (pedidoExistente == null)
            {
                throw new ApplicationException("ERRO_PEDIDO_NAO_ENCONTRADO");
            }
            else
            {
                pedidoExistente.Qtdade = pedidoNovo.Qtdade;
                pedidoExistente.Pagou = pedidoNovo.Pagou;
                pedidoExistente.Pegou = pedidoNovo.Pegou;
            }

            PedidoRepositorio.Atualizar(todosPedidos, numeroSemana);
        }

        /// <summary>
        /// Lista todos os pedidos cadastrados.
        /// </summary>
        /// <returns></returns>
        public List<Pedido> Listar()
        {
            return PedidoRepositorio.Listar();
        }

        /// <summary>
        /// Lista todos os pedidos cadastrados de uma determinada semana.
        /// </summary>
        /// <param name="numeroSemana"></param>
        /// <returns></returns>
        public List<Pedido> Listar(int numeroSemana)
        {
            try
            {
                return PedidoRepositorio.Listar(numeroSemana);
            }
            catch (FileNotFoundException)
            {
                throw new ApplicationException("ERRO_SEMANA_NAO_ENCONTRADA");
            }
            catch (Exception)
            {
                throw new ApplicationException("ERRO_MSG_PADRAO");
            }
        }

        /// <summary>
        /// Consulta um pedido especifico da semana atual pelo login.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Pedido ConsultarPedido(string login)
        {
            return Listar().FirstOrDefault(x => string.Equals(x.Login, login));
        }

        /// <summary>
        /// Consulta um pedido especifico da semana desejada pelo login.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="numeroSemana"></param>
        /// <returns></returns>
        public Pedido ConsultarPedido(string login, int numeroSemana)
        {
            if (numeroSemana == 0)
            {
                return PedidoRepositorio.Listar().FirstOrDefault(x => string.Equals(x.Login, login));
            }
            else
            {
                return PedidoRepositorio.Listar(numeroSemana).FirstOrDefault(x => string.Equals(x.Login, login));
            }
        }

        #region Validações

        /// <summary>
        /// Verifica se já existe um pedido para o login.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private bool ValidaPedidoExistente(string login)
        {
            var jaExistePedido = Listar().Any(x => string.Equals(x.Login, login));
            if (jaExistePedido)
            {
                throw new ApplicationException("ERRO_PEDIDO_EXISTENTE");
            }
            return jaExistePedido;
        }

        /// <summary>
        /// Verifica se o login do usuario é valido.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private bool ValidaLoginUsuario(string login)
        {
            var isValid = ServicoUsuario.ValidarLogin(login);
            if (!isValid)
            {
                throw new ApplicationException("ERRO_EMAIL_INVALIDO");
            }
            return isValid;
        }

        /// <summary>
        /// Verifica a quantidade minima e máxima de pães de queijo permitido.
        /// </summary>
        /// <param name="qtdade"></param>
        private void ValidaQtdadePaes(int qtdade)
        {
            if (qtdade < 1 || qtdade > 2)
            {
                throw new ApplicationException("ERRO_QTDADE_INVALIDA");
            }
        }

        #endregion
    }
}
