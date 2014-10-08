using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using Br.Com.BiscoitinhosVovoLiva.Repositorio;
using Newtonsoft.Json;

namespace Br.Com.BiscoitinhosVovoLiva.RepositorioJSON
{
    public class PedidoRepositorio : RepositorioJSON<Pedido>, IPedidoRepositorio
    {
        #region Constantes

        private const string CAMINHO_JSON = @"C:\BDVLiva\json";
        private const string PADRAO_NOME_JSON = "{0}_pedidos_semana_{1}.json";

        #endregion

        public PedidoRepositorio()
        {
            VerificaExistenciaDiretorio();
            VerificaExistenciaArquivoSemana();
        }

        public void Salvar(Pedido pedido)
        {
            var json = JsonConvert.SerializeObject(pedido);
            using (var writer = File.AppendText(RecuperaCaminhoCompleto(GetNumeroSemana())))
            {
                writer.WriteLine(json);
            }
        }

        public List<Pedido> Listar()
        {
            var lista = new List<Pedido>();
            foreach (var linha in File.ReadLines(RecuperaCaminhoCompleto(GetNumeroSemana())))
            {
                var pedido = JsonConvert.DeserializeObject<Pedido>(linha);
                lista.Add(pedido);
            }
            return lista;
        }

        public void Atualizar(List<Pedido> todosPedidos)
        {
            var sbTodosPedidos = new StringBuilder();
            foreach (var pedido in todosPedidos)
            {
                sbTodosPedidos.AppendLine(JsonConvert.SerializeObject(pedido));
            }
            File.WriteAllText(RecuperaCaminhoCompleto(GetNumeroSemana()), sbTodosPedidos.ToString());
        }

        public void Atualizar(List<Pedido> todosPedidos, int numeroSemana)
        {
            var sbTodosPedidos = new StringBuilder();
            foreach (var pedido in todosPedidos)
            {
                sbTodosPedidos.AppendLine(JsonConvert.SerializeObject(pedido));
            }
            File.WriteAllText(RecuperaCaminhoCompleto(numeroSemana.ToString()), sbTodosPedidos.ToString());
        }

        public List<Pedido> Listar(int numeroSemana)
        {
            var lista = new List<Pedido>();
            foreach (var linha in File.ReadLines(RecuperaCaminhoCompleto(numeroSemana.ToString())))
            {
                var pedido = JsonConvert.DeserializeObject<Pedido>(linha);
                lista.Add(pedido);
            }
            return lista;
        }

        #region Metodos Auxiliares

        private void VerificaExistenciaArquivoSemana()
        {
            if (!File.Exists(RecuperaCaminhoCompleto(GetNumeroSemana())))
            {
                File.Create(RecuperaCaminhoCompleto(GetNumeroSemana()));
            }
        }

        private void VerificaExistenciaDiretorio()
        {
            if (!Directory.Exists(CAMINHO_JSON))
            {
                Directory.CreateDirectory(CAMINHO_JSON);
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

        private string RecuperaNomeArquivoJson(string numeroSemana)
        {
            return string.Format(PADRAO_NOME_JSON, DateTime.Now.Year, numeroSemana);
        }

        private string RecuperaCaminhoCompleto(string numeroSemana)
        {
            return string.Format(@"{0}\{1}", CAMINHO_JSON, RecuperaNomeArquivoJson(numeroSemana));
        }

        #endregion
    }
}
