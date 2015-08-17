using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Br.Com.BiscoitinhosVovoLiva.Entidade.Mapemanetos
{
    public class PedidoMap : ClassMap<Pedido>
    {
        public PedidoMap()
        {
            Table("Pedido");
            Id(x => x.Id, "Id");
            Map(x => x.Login, "Login");
            Map(x => x.Email, "Email");
            Map(x => x.Qtdade, "Qtdade");
            Map(x => x.Pegou, "Pegou");
            Map(x => x.Pagou, "Pagou");
            Map(x => x.Data, "Data");
            Map(x => x.NumeroSemana, "NumeroSemana");
            Map(x => x.Ano, "Ano");
        }
    }
}
