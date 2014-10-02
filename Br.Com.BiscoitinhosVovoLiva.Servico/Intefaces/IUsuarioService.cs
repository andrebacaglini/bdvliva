
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

        /// <summary>
        /// Verifica se o usuario que esta logando é o administrador (andre.bacaglini)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        bool IsAdmin(string login, string senha);
    }
}
