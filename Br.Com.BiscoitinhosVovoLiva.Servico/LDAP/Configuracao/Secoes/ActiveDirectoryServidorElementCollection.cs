using System;
using System.Configuration;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.LDAP.Configuracao.Secoes
{
    public class ActiveDirectoryServidorElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ActiveDirectoryServidorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var servidor = element as ActiveDirectoryServidorElement;
            return servidor == null ? String.Empty : servidor.Nome;
        }
    }
}
