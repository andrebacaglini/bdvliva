using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using Br.Com.BiscoitinhosVovoLiva.Intranet.App_LocalResources;
using Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet.Controllers
{
    public class HomeController : Controller
    {
        #region Constantes

        private const int ID_ERRO = -1;
        private const int ID_SUCESSO = 0;

        #endregion

        #region Servicos

        public IPedidoService ServicoPedido { get; set; }
        public IUsuarioService ServicoUsuario { get; set; }

        #endregion

        #region Ações

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pedidos()
        {
            return View();
        }

        #endregion

        #region Ajax

        [HttpPost]
        public JsonResult RealizarPedido(FormCollection form)
        {
            try
            {
                var pedido = ExtrairPedidoForm(form);
                var loginValido = ServicoUsuario.ValidarLogin(pedido.Login);

                if (loginValido)
                {
                    ServicoPedido.Salvar(pedido);
                }
                else
                {
                    throw new ApplicationException(Mensagens.ERRO_EMAIL_INVALIDO);
                }
                return TrataJsonResult(ID_SUCESSO, Mensagens.SUCESSO_PEDIDO_REALIZADO);
            }
            catch (Exception e)
            {
                return TrataJsonResult(ID_ERRO, e);
            }
        }

        [HttpPost]
        public JsonResult AtualizarPedido(FormCollection collection)
        {
            try
            {
                //TODO: Verificar e-mail da cast. - Exceção email invalido
                if (!collection["email"].Any())
                {
                    throw new ApplicationException("email invalido");
                }
                // TODO: Identificar semana
                // TODO: Verificar se email já possui pedido para semana - Exceção caso não tenha pedido
                // TODO: Atualizar Pedido para semana
                return TrataJsonResult(ID_SUCESSO, "Sucesso Edit");
            }
            catch (Exception e)
            {
                return TrataJsonResult(ID_ERRO, e);
            }
        }

        public JsonResult ListarPedidos()
        {
            var paginaAtual = Convert.ToInt32(Request.Params["current"]);
            var qtdadePorPagina = Convert.ToInt32(Request.Params["rowCount"]);
            var parametroConsulta = Request.Params["searchPhrase"];

            //var listaPedidos = CriaListaMOCK();
            var listaPedidos = ServicoPedido.Listar();

            OrdenaPedidos(ref listaPedidos);

            var listaFiltradaPaginada = new List<Pedido>();
            var totalRegistros = 0;

            if (qtdadePorPagina != -1)
            {
                var toSkip = paginaAtual == 1 ? 0 : (paginaAtual - 1) * qtdadePorPagina;

                var listaFiltrada = listaPedidos
                    .Where(FiltroEmail(parametroConsulta))
                    .ToList();

                totalRegistros = listaFiltrada.Count;

                listaFiltradaPaginada = listaFiltrada
                    .Skip(toSkip)
                    .Take(qtdadePorPagina)
                    .ToList();
            }
            else
            {
                listaFiltradaPaginada = listaPedidos
                    .Where(FiltroEmail(parametroConsulta))
                    .ToList();
                totalRegistros = listaFiltradaPaginada.Count;
            }

            var jsonResponse = new
            {
                current = paginaAtual,
                rowCount = qtdadePorPagina,
                rows = listaFiltradaPaginada.ToArray(),
                total = totalRegistros
            };

            return Json(jsonResponse);
        }

        #endregion

        #region Metodos Auxiliares

        /// <summary>
        /// Extrai os dados do formulario do pedido criando uma nova instancia de Pedido.
        /// </summary>
        /// <param name="form">Formulário do pedido</param>
        /// <returns>Novo Pedido</returns>        
        protected Pedido ExtrairPedidoForm(FormCollection form)
        {
            try
            {
                var p = new Pedido();
                p.Email = form["email"].ToString();
                p.Login = p.Email.Split('@')[0];
                p.Qtdade = Convert.ToInt32(form["qtd"]);
                return p;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Mensagens.ERRO_REQ_FORM_COLLECTION, ex);
            }
        }

        /// <summary>
        /// Ordena os pedidos de acordo com a coluna selecionada.
        /// </summary>
        /// <param name="listaPedidos">Todos os pedidos para ordenação</param>
        /// <returns>Lista ordenada</returns>
        private List<Pedido> OrdenaPedidos(ref List<Pedido> listaPedidos)
        {
            if (!string.IsNullOrEmpty(Request.Params["sort[Email]"]))
            {
                switch (Request.Params["sort[Email]"])
                {
                    case "desc":
                        listaPedidos = listaPedidos.OrderByDescending(x => x.Email).ToList();
                        break;
                    default://asc
                        listaPedidos = listaPedidos.OrderBy(x => x.Email).ToList();
                        break;
                }
            }
            else
            {
                switch (Request.Params["sort[Qtdade]"])
                {
                    case "desc":
                        listaPedidos = listaPedidos.OrderByDescending(x => x.Qtdade).ToList();
                        break;
                    default://asc
                        listaPedidos = listaPedidos.OrderBy(x => x.Qtdade).ToList();
                        break;
                }
            }


            return listaPedidos;
        }

        /// <summary>
        /// Cria expressão de filtro pelo e-mail.
        /// Caso não tenha filtro, a expressão retornará true.
        /// </summary>
        /// <param name="parametroConsulta">Parametro fornecido na grid.</param>
        /// <returns>Expressão de filtro pelo e-mail.</returns>
        private static Func<Pedido, bool> FiltroEmail(string parametroConsulta)
        {
            Func<Pedido, bool> retorno = x => true;
            if (!string.IsNullOrEmpty(parametroConsulta))
            {
                retorno = x => x.Email.Contains(parametroConsulta);
            }
            return retorno;
        }

        /// <summary>
        /// Trata as respostas json de acordo com o sucesso ou fracasso das ações.
        /// </summary>
        /// <param name="id">Identificador de sucesso ou erro.</param>
        /// <param name="data">Objeto de acordo com o identificador. 
        /// Sucesso - String
        /// Erro - Exception</param>
        /// <returns></returns>
        public JsonResult TrataJsonResult(int id, object data)
        {
            var retorno = new JsonResult();
            retorno.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            switch (id)
            {
                case ID_SUCESSO:
                    var msg = data as string;
                    retorno.Data = new { id = id, msg = msg };
                    break;
                case ID_ERRO:
                    // Exceções tratadas devem sempre ser ApplicationException
                    if (data.GetType() == typeof(ApplicationException))
                    {
                        var excecao = data as ApplicationException;
                        retorno.Data = new { id = id, msg = excecao.Message };
                    }
                    else
                    {
                        var excecao = data as Exception;
                        retorno.Data = new { id = id, msg = Mensagens.ERRO_MSG_PADRAO };
                    }

                    break;
                default:
                    break;
            }
            return retorno;
        }

        #endregion
    }
}
