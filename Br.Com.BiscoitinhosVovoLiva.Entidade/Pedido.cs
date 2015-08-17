using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.Com.BiscoitinhosVovoLiva.Entidade
{
    [Serializable]
    public class Pedido
    {
        public virtual int Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Email { get; set; }
        public virtual int Qtdade { get; set; }
        public virtual bool Pegou { get; set; }
        public virtual bool Pagou { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual string DataString { get { return Data.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture); } }
        public virtual int NumeroSemana { get; set; }
        public virtual int Ano { get; set; }
    }
}
