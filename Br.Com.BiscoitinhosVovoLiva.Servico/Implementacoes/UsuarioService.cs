using System;
using Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces;
using Br.Com.BiscoitinhosVovoLiva.Servico.LDAP;
//using Br.Com.BiscoitinhosVovoLiva.Servico.LDAP;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.Implementacoes
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        #region Propriedades

        public ActiveDirectoryUtility ADUtility { get; set; }

        #endregion

        /// <summary>
        /// Valida o login do usuario no ActiveDirectory.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool ValidarLogin(string login)
        {
            try
            {                
                var usuarioValido = ADUtility.ValidarUsuario(login);
                return usuarioValido;
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}
