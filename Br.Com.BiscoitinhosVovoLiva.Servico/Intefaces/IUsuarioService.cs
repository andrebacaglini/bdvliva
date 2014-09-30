using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces
{
    public interface IUsuarioService
    {
        /// <summary>
        /// Valida o login do usuario.
        /// </summary>
        /// <param name="login">Login usuario</param>
        /// <returns></returns>
        bool ValidarLogin(string login);
    }
}
