using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Proxy;
using NHibernate.Criterion;

namespace Br.Com.BiscoitinhosVovoLiva.RepositorioSQLSERVER
{
    public class RepositorioSQLSERVER<T> where T : Pedido
    {
        protected ISession Session { get; set; }

        public RepositorioSQLSERVER(ISession session)
        {
            this.Session = session;
        }

        protected T ObterPorId(int id)
        {
            return this.Session.Query<T>().FirstOrDefault(x => x.Id == id);
        }
    }
}
