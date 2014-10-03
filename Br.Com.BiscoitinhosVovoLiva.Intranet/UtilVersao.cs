using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Br.Com.BiscoitinhosVovoLiva.Intranet
{
    public static class UtilVersao
    {
        private static Version current = Assembly.GetExecutingAssembly().GetName().Version;

        public static string RecuperaCurrentVersion()
        {
            return current.ToString();
        }

        public static string RecuperaCurrentBuildDate()
        {
            var date = new DateTime(2000, 1, 1).AddDays(current.Build).AddSeconds(current.Revision * 2);
            return date.ToString("dd/MM/yy HH:mm");
        }

        public static string RecuperaVersionAndBuildDate()
        {
            return string.Format("{0} - {1}", RecuperaCurrentVersion(), RecuperaCurrentBuildDate());
        }
    }
}