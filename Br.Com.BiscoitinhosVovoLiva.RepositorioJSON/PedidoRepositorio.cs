using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using Br.Com.BiscoitinhosVovoLiva.Repositorio;
using Newtonsoft.Json;
using System.Linq;

namespace Br.Com.BiscoitinhosVovoLiva.RepositorioJSON
{
    public class PedidoRepositorio : RepositorioJSON<Pedido>, IPedidoRepositorio
    {
        private const string CAMINHO_XML = @"C:\BDVLiva\json";
        private const string PADRAO_NOME_XML = "{0}_pedidos_semana_{1}.json";

        private string CaminhoCompleto
        {
            get
            {
                return string.Format(@"{0}\{1}", CAMINHO_XML, NomeArquivoXml);
            }
        }

        private string NomeArquivoXml
        {
            get
            {
                return string.Format(PADRAO_NOME_XML, DateTime.Now.Year, GetNumeroSemana());
            }
        }

        public PedidoRepositorio()
        {
            VerificaExistenciaDiretorio();
            VerificaExistenciaArquivoSemana();
        }

        private void VerificaExistenciaArquivoSemana()
        {
            if (!File.Exists(CaminhoCompleto))
            {
                File.Create(CaminhoCompleto);
            }
        }

        private void VerificaExistenciaDiretorio()
        {
            if (!Directory.Exists(CAMINHO_XML))
            {
                Directory.CreateDirectory(CAMINHO_XML);
            }
        }

        private string GetNumeroSemana()
        {
            var data = DateTime.Now.Date;
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(data);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                data = data.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(data, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString();
        }

        public void Salvar(Pedido pedido)
        {
            var json = JsonConvert.SerializeObject(pedido);
            using (var writer = File.AppendText(CaminhoCompleto))
            {
                writer.WriteLine(json);
            }
        }

        public int RecuperaUltimoId()
        {
            var lista = new List<Pedido>();

            foreach (var linha in File.ReadLines(CaminhoCompleto))
            {
                var pedido = JsonConvert.DeserializeObject<Pedido>(linha);
                lista.Add(pedido);
            }

            var ultimoId = lista.Any() ? lista.OrderByDescending(x => x.IdPedido).FirstOrDefault().IdPedido : 0;

            return ultimoId;
        }

        public List<Pedido> Listar()
        {
            var lista = new List<Pedido>();
            foreach (var linha in File.ReadLines(CaminhoCompleto))
            {
                var pedido = JsonConvert.DeserializeObject<Pedido>(linha);
                lista.Add(pedido);
            }
            return lista;
        }
    }
}
