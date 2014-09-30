using System;
using System.Configuration;

namespace Br.Com.BiscoitinhosVovoLiva.Servico.LDAP.Configuracao.Secoes
{
    public class ActiveDirectoryConfigurationSection : ConfigurationSection
    {
        private const string NomeServidorPrincipalAttribute = "nomeServidorPrincipal";
        private const string CampoLoginAttribute = "campoLogin";
        private const string GruposElement = "grupos";
        private const string ServidoresAdicionaisElement = "servidoresAdicionais";

        [ConfigurationProperty(NomeServidorPrincipalAttribute, IsRequired = true)]
        public string NomeServidorPrincipal
        {
            get
            {
                return this[NomeServidorPrincipalAttribute].ToString();
            }
            set
            {
                this[NomeServidorPrincipalAttribute] = value;
            }
        }

        [ConfigurationProperty(CampoLoginAttribute, IsRequired = true)]
        public string CampoLogin
        {
            get
            {
                return this[CampoLoginAttribute].ToString();
            }
            set
            {
                this[CampoLoginAttribute] = value;
            }
        }

        [ConfigurationProperty(GruposElement, IsRequired = false)]
        public ActiveDirectoryGrupoElementCollection Grupos
        {
            get
            {
                return this[GruposElement] as ActiveDirectoryGrupoElementCollection;
            }
            set
            {
                this[GruposElement] = value;
            }
        }

        [ConfigurationProperty(ServidoresAdicionaisElement, IsRequired = false)]
        public ActiveDirectoryServidorElementCollection ServidoresAdicionais
        {
            get
            {
                return this[ServidoresAdicionaisElement] as ActiveDirectoryServidorElementCollection;
            }
            set
            {
                this[ServidoresAdicionaisElement] = value;
            }
        }
    }
}
