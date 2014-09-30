using System.Configuration;
using Br.Com.BiscoitinhosVovoLiva.Servico.LDAP.Configuracao.Secoes;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.LDAP.Configuracao
{
    public static class ActiveDirectoryConfiguracao
    {
        private const string SecaoConfiguracaoActiveDirectory = "activeDirectoryConfiguracao";

        public static ActiveDirectoryConfigurationSection ConfiguracoesActiveDirectory()
        {
            return ConfigurationManager.GetSection(SecaoConfiguracaoActiveDirectory) as ActiveDirectoryConfigurationSection;
        }
    }
}