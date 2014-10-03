using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet.Controllers
{
    public class AccountController : Controller
    {
        #region Servicos

        public IUsuarioService UsuarioService { get; set; }

        #endregion

        #region Ações

        public ActionResult Login(string ReturnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string senha, string ReturnUrl)
        {
            try
            {
                if (UsuarioService.ValidarLogin(usuario, senha))
                {
                    CriaCookieAutenticacao(usuario);
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        var array = ReturnUrl.Split('/');
                        var action = array.Length > 2 ? array[2] : "Index";
                        var controller = array[1];
                        return RedirectToAction(action, controller);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View("AcessoNegado");
            }
            catch (Exception)
            {
                return View("AcessoNegado");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Metodos Auxiliares

        /// <summary>
        /// Cria cookie de autenticação para usuario.
        /// </summary>
        /// <param name="usuario"></param>
        private void CriaCookieAutenticacao(string usuario)
        {
            var authTicket = new FormsAuthenticationTicket(1, usuario, DateTime.Now,
                                      DateTime.Now.AddSeconds(30), true, "");

            string cookieContents = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
            {
                Expires = authTicket.Expiration,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (HttpContext != null)
            {
                HttpContext.Response.Cookies.Add(cookie);
            }
        }

        #endregion

    }
}
