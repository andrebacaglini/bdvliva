using System;
using System.Configuration;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.LDAP.Configuracao.Secoes
{
    public class ActiveDirectoryServidorElement : ConfigurationElement
    {
        private const string NomeServidorAttribute = "nome";

        [ConfigurationProperty(NomeServidorAttribute, IsRequired = true)]
        public string Nome
        {
            get
            {
                return this[NomeServidorAttribute].ToString();
            }
            set
            {
                this[NomeServidorAttribute] = value;
            }
        }
    }
}
