using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using Br.Com.BiscoitinhosVovoLiva.Intranet.App_LocalResources;
using Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces;
using log4net;
using Newtonsoft.Json;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet.Controllers
{
    [Authorize]
    public class PaoDeQueijoController : Controller
    {
        #region Constantes

        private const int ID_ERRO = -1;
        private const int ID_SUCESSO = 0;

        #endregion

        #region Servicos

        public ILog Logger { get; set; }
        public IPedidoService ServicoPedido { get; set; }

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
                ServicoPedido.Salvar(pedido);
                EnviarEmail(Mensagens.SUCESSO_EMAIL_REALIZAR, pedido);
                return TrataJsonResult(ID_SUCESSO, Mensagens.SUCESSO_PEDIDO_REALIZADO);
            }
            catch (Exception e)
            {
                return TrataJsonResult(ID_ERRO, e);
            }
        }

        [HttpPost]
        public JsonResult AtualizarPedido(FormCollection form)
        {
            try
            {
                var pedido = ExtrairPedidoForm(form);
                ServicoPedido.Atualizar(pedido);
                EnviarEmail(Mensagens.SUCESSO_EMAIL_ATUALIZAR, pedido);
                return TrataJsonResult(ID_SUCESSO, Mensagens.SUCESSO_PEDIDO_ATUALIZADO);
            }
            catch (Exception e)
            {
                return TrataJsonResult(ID_ERRO, e);
            }
        }

        [HttpPost]
        public JsonResult Pagar(string login, bool pagou, int numeroSemana)
        {
            try
            {
                var pedido = ServicoPedido.ConsultarPedido(login, numeroSemana);
                pedido.Pagou = pagou;
                ServicoPedido.Atualizar(pedido, numeroSemana);

                var retorno = new JsonResult();
                retorno.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                retorno.Data = new { Login = login, pagou = pagou };
                return retorno;
            }
            catch (Exception e)
            {
                return TrataJsonResult(ID_ERRO, e);
            }
        }

        [HttpPost]
        public JsonResult Pegar(string login, bool pegou, int numeroSemana)
        {
            try
            {
                var pedido = ServicoPedido.ConsultarPedido(login, numeroSemana);
                pedido.Pegou = pegou;
                ServicoPedido.Atualizar(pedido, numeroSemana);

                var retorno = new JsonResult();
                retorno.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                retorno.Data = JsonConvert.SerializeObject(pedido);
                return retorno;
            }
            catch (Exception e)
            {
                return TrataJsonResult(ID_ERRO, e);
            }
        }

        //TODO: Refatorar/Melhorar
        [HttpPost]
        public JsonResult ListarPedidos()
        {
            var paginaAtual = Convert.ToInt32(Request.Params["current"]);
            var qtdadePorPagina = Convert.ToInt32(Request.Params["rowCount"]);
            var parametroConsulta = Request.Params["searchPhrase"];
            var numeroSemana = Convert.ToInt32(Request.Params["numeroSemana"]);

            try
            {
                var listaPedidos = ConsultaPedidos(numeroSemana);

                var totalDePaes = 0;
                listaPedidos.ForEach(x => totalDePaes += x.Qtdade);

                OrdenarPedidos(ref listaPedidos);

                var totalRegistros = 0;
                var listaFiltradaPaginada = FiltrarPaginarPedidos(listaPedidos, ref totalRegistros);

                return CriaJsonRetornoJqueryBootgrid(paginaAtual, qtdadePorPagina, numeroSemana, totalDePaes, totalRegistros, listaFiltradaPaginada);
            }
            catch (Exception)
            {
                return CriaJsonRetornoJqueryBootgrid(0, 0, numeroSemana, 0, 0, new List<Pedido>());
            }
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
                p.Email = string.Format("{0}@cast.com.br", User.Identity.Name);
                p.Login = User.Identity.Name;
                p.Qtdade = Convert.ToInt32(form["qtd"]);
                p.Pegou = false;
                p.Pagou = false;
                p.Data = DateTime.Now;
                return p;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("ERRO_REQ_FORM_COLLECTION", ex);
            }
        }

        /// <summary>
        /// Ordena os pedidos de acordo com a coluna selecionada.
        /// </summary>
        /// <param name="listaPedidos">Todos os pedidos para ordenação</param>
        /// <returns>Lista ordenada</returns>
        private List<Pedido> OrdenarPedidos(ref List<Pedido> listaPedidos)
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
                        var mensagem = Mensagens.ResourceManager.GetString(excecao.Message);
                        mensagem = string.IsNullOrEmpty(mensagem) ? "Mensagem de erro não encontrada." : mensagem;
                        retorno.Data = new { id = id, msg = mensagem };
                        Logger.Error(excecao.Message, excecao);
                    }
                    else
                    {
                        var excecao = data as Exception;
                        retorno.Data = new { id = id, msg = Mensagens.ERRO_MSG_PADRAO };
                        Logger.Error(excecao.Message, excecao);
                    }

                    break;
                default:
                    break;
            }
            return retorno;
        }

        private List<Pedido> ConsultaPedidos(int numeroSemana)
        {
            var listaPedidos = new List<Pedido>();
            if (numeroSemana != 0)
            {
                listaPedidos = ServicoPedido.Listar(numeroSemana);
            }
            else
            {
                listaPedidos = ServicoPedido.Listar();
            }
            return listaPedidos;
        }

        private List<Pedido> FiltrarPaginarPedidos(List<Pedido> listaPedidos, ref int totalRegistros)
        {
            var listaFiltradaPaginada = new List<Pedido>();
            var paginaAtual = Convert.ToInt32(Request.Params["current"]);
            var qtdadePorPagina = Convert.ToInt32(Request.Params["rowCount"]);
            var parametroConsulta = Request.Params["searchPhrase"];

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
            return listaFiltradaPaginada;
        }

        private int GetNumeroSemana()
        {
            var data = DateTime.Now.Date;
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(data);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                data = data.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(data, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private JsonResult CriaJsonRetornoJqueryBootgrid(int paginaAtual, int qtdadePorPagina, int numeroSemana, int totalDePaes, int totalRegistros, List<Pedido> listaFiltradaPaginada)
        {
            var jsonResponse = new
            {
                current = paginaAtual,
                rowCount = qtdadePorPagina,
                rows = listaFiltradaPaginada.ToArray(),
                total = totalRegistros,
                totalpdq = totalDePaes,
                semanaAtual = numeroSemana == 0 ? GetNumeroSemana() : numeroSemana

            };
            return Json(jsonResponse);
        }

        private void EnviarEmail(string mensagem, Pedido pedido)
        {
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    using (var msg = new MailMessage("andre.bacaglini@cast.com.br", pedido.Email))
                    {
                        var texto = pedido.Qtdade == 1 ? "pão" : "pães";
                        msg.IsBodyHtml = true;
                        msg.Subject = "[PãoDeQueijo] - Pedido realizado com sucesso!";
                        msg.Body = string.Format(mensagem, pedido.Login, pedido.Qtdade, texto, DateTime.Now.ToShortDateString());
                        client.Send(msg);
                    }
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("ERRO_ENVIAR_EMAIL");
            }
        }

        #endregion

    }
}
