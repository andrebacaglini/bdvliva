using System;
using Br.Com.BiscoitinhosVovoLiva.Servico.Intefaces;
using Br.Com.BiscoitinhosVovoLiva.Servico.LDAP;

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
                return ADUtility.ValidarUsuario(login);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica se o usuario que esta logando é o administrador (andre.bacaglini)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        public bool ValidarLogin(string login, string senha)
        {
            try
            {
                return ADUtility.ValidarUsuario(login, senha);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
