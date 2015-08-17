using System;
using System.Collections.Generic;
using System.Globalization;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using Br.Com.BiscoitinhosVovoLiva.Repositorio;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Proxy;
using NHibernate.Criterion;
using System.Linq;

namespace Br.Com.BiscoitinhosVovoLiva.RepositorioSQLSERVER
{
    public class PedidoRepositorio : RepositorioSQLSERVER<Pedido>, IPedidoRepositorio
    {
        public PedidoRepositorio(ISession session) : base(session) { }

        public void Salvar(Pedido pedido)
        {
            //var json = JsonConvert.SerializeObject(pedido);
            //using (var writer = File.AppendText(RecuperaCaminhoCompleto(GetNumeroSemana(), GetAno())))
            //{
            //    writer.WriteLine(json);
            //}
            this.Session.SaveOrUpdate(pedido);
        }

        public Pedido ObterPorId(int id)
        {
            return base.ObterPorId(id);
        }

        public List<Pedido> Listar()
        {
            return this.Session.Query<Pedido>()
                .Where(x => x.NumeroSemana == GetNumeroSemana() && x.Ano == GetAno())
                .ToList();
        }

        public List<Pedido> Listar(int numeroSemana)
        {
            return this.Session.Query<Pedido>()
                .Where(x => x.NumeroSemana == numeroSemana && x.Ano == GetAno()).ToList();
        }

        #region Metodos Auxiliares

        private int GetNumeroSemana()
        {
            var data = DateTime.Now.Date;
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(data);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                data = data.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(data, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private int GetAno()
        {
            var data = DateTime.Now.Date;
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(data);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                data = data.AddDays(3);
            }
            return data.Year;
        }

        #endregion

        public void Atualizar(List<Pedido> todosPedidos)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(List<Pedido> todosPedidos, int numeroSemana)
        {
            throw new NotImplementedException();
        }
    }
}
