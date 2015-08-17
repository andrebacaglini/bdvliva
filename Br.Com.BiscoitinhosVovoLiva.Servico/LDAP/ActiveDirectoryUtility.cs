using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Br.Com.BiscoitinhosVovoLiva.Servico.LDAP.Configuracao;
using Br.Com.BiscoitinhosVovoLiva.Servico.LDAP.Configuracao.Secoes;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.LDAP
{
    public class ActiveDirectoryUtility
    {
        #region Constantes

        private const string LdapPadraoServidor = "LDAP://{0}";
        private const string LdapPadraoFiltroLogin = "(&({0}={1}))";
        private const string ErroActiveDirectory = "";

        #endregion

        #region Variáveis

        private readonly ActiveDirectoryConfigurationSection _config;
        private string _servidorConsulta;

        #endregion

        public ActiveDirectoryUtility()
        {
            _config = ActiveDirectoryConfiguracao.ConfiguracoesActiveDirectory();
            CarregarNomesServidores();
        }

        #region Metodos Auxiliares

        private void CarregarNomesServidores()
        {
            _servidorConsulta = _config.NomeServidorPrincipal;
        }

        private string LdapServerUrl(string nomeServidor)
        {
            return String.Format(LdapPadraoServidor, nomeServidor);
        }

        private DirectoryEntry DiretorioRaiz(string nomeServidor, string escopo)
        {
            var url = LdapServerUrl(nomeServidor);
            if (!String.IsNullOrEmpty(escopo))
            {
                url += "/" + escopo;
            }
            return new DirectoryEntry(url);
        }

        private DirectorySearcher PesquisadorDiretorio(DirectoryEntry diretorioRaiz, string filtro)
        {
            return new DirectorySearcher
            {
                SearchRoot = diretorioRaiz,
                ClientTimeout = new TimeSpan(0, 0, 15),
                Filter = filtro,
                SearchScope = SearchScope.Subtree,
                Sort =
                {
                    Direction = SortDirection.Ascending,
                    PropertyName = _config.CampoLogin
                }
            };
        }

        private SearchResultCollection PesquisarRegistros(string servidor, string filtro, string escopo)
        {
            SearchResultCollection resultado;
            using (var diretorioRaiz = DiretorioRaiz(servidor, escopo))
            {
                diretorioRaiz.AuthenticationType = AuthenticationTypes.None;
                using (var pesquisador = PesquisadorDiretorio(diretorioRaiz, filtro))
                {
                    resultado = pesquisador.FindAll();
                }
            }
            return resultado;
        }

        internal SearchResult PesquisarRegistro(string servidor, string filtro, string escopo)
        {
            SearchResult resultado = null;
            using (var registros = PesquisarRegistros(servidor, filtro, escopo))
            {
                if (registros != null)
                {
                    resultado = registros.Cast<SearchResult>().FirstOrDefault();
                }
            }
            return resultado;
        }

        internal bool ValidarRegistro(DirectoryEntry registro, string senha)
        {
            registro.Username = RemoveUrlServidor(registro.Path);
            registro.Password = senha;
            registro.AuthenticationType = AuthenticationTypes.None;
            try
            {
                registro.RefreshCache();
                return true;
            }
            catch (DirectoryServicesCOMException ex)
            {
                if (ex.ExtendedError == -2146893048)
                {
                    throw ex;
                }
                throw ex;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal string RemoveUrlServidor(string caminhoLdap)
        {
            var valores = caminhoLdap.Split(new[] { '/' });
            return valores.GetValue(valores.Length - 1).ToString();
        }
        #endregion

        #region Metodos Publicos

        public bool ValidarUsuario(string login)
        {
            var filtro = String.Format(LdapPadraoFiltroLogin, _config.CampoLogin, login);
            var grupos = _config.Grupos.Cast<ActiveDirectoryGrupoElement>();

            List<SearchResult> registros = null;
            try
            {
                registros = grupos
                    .Select(grp => PesquisarRegistro(_servidorConsulta, filtro, grp.Nome))
                    .ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (registros == null || registros.All(reg => reg == null))
            {
                throw new ApplicationException("ERRO_USUARIO_NAO_ENCONTRADO");
            }

            return true;
        }

        public bool ValidarUsuario(string login, string senha)
        {
            var filtro = String.Format(LdapPadraoFiltroLogin, _config.CampoLogin, login);
            var grupos = _config.Grupos.Cast<ActiveDirectoryGrupoElement>();

            List<SearchResult> registros = null;
            
            try
            {                
                registros = grupos
                    .Select(grp => PesquisarRegistro(_servidorConsulta, filtro, grp.Nome))
                    .ToList();

            }
            catch (Exception ex)
            {
                throw ex;                
            }

            if (registros == null || registros.All(reg => reg == null))
            {
                throw new ApplicationException("ERRO_USUARIO_NAO_ENCONTRADO");
            }

            return registros.Any(x => x != null && ValidarRegistro(x.GetDirectoryEntry(), senha));
        }

        #endregion
    }
}
