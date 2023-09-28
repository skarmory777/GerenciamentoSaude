using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Extensions
{
    public static partial class FuncoesGlobais
    {
        public static SelectList MontarHorarios(AgendamentoConsultaMedicoDisponibilidadeDto agendamento, DateTime horaAgendamento)
        {
            var horaIni = new DateTime(horaAgendamento.Year, horaAgendamento.Month, horaAgendamento.Day, agendamento.HoraInicio.Hour, agendamento.HoraInicio.Minute, 00);
            var horaFim = new DateTime(horaAgendamento.Year, horaAgendamento.Month, horaAgendamento.Day, agendamento.HoraFim.Hour, agendamento.HoraFim.Minute, 00);
            var intervaloMinutos = agendamento.Intervalo.IntervaloMinutos;
            var intervaloAtendmentos = agendamento.Intervalo.AtendimentosPorHora;
            var teste = Thread.CurrentThread.CurrentCulture;
            var horarios = new List<DateTime>();
            var horaLoop = horaIni;
            horarios.Add(horaIni);
            while (horaLoop <= horaFim)
            {
                horaLoop = horaLoop.AddMinutes(intervaloMinutos);
                if (horaLoop < horaFim)
                {
                    horarios.Add(horaLoop);
                }
            }
            var intervalosAgendamento = new SelectList(horarios.Select(m => new { HoraAgendamento = teste.ToString().ToUpper() == "PT-BR" ? m.ToString("dd/MM/yyyy HH:mm") : teste.ToString().ToUpper() == "EN" ? m.ToString("MM/dd/yyyy HH:mm") : m.ToString("yyyy/MM/dd HH:mm"), Nome = string.Format("{0:D2}:{1:D2}", m.Hour, m.Minute) }), "HoraAgendamento", "Nome", horaAgendamento);
            return intervalosAgendamento;
        }
        public static List<DateTime> Horarios()
        {
            var horaIni = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 00);
            var horaFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 00);
            var intervaloMinutos = 1;
            var teste = Thread.CurrentThread.CurrentCulture;
            var horarios = new List<DateTime>();
            var horaLoop = horaIni;
            horarios.Add(horaLoop);
            while (horaLoop <= horaFim)
            {
                horaLoop = horaLoop.AddMinutes(intervaloMinutos);
                if (horaLoop < horaFim)
                {
                    horarios.Add(horaLoop);
                }
            }

            //var intervalosAgendamento = new SelectList(horarios.Select(m => new { Value = teste.ToString().ToUpper() == "PT-BR" ? m.ToString("dd/MM/yyyy HH:mm") : teste.ToString().ToUpper() == "EN" ? m.ToString("MM/dd/yyyy HH:mm") : m.ToString("yyyy/MM/dd HH:mm"), Text = string.Format("{0:D2}:{1:D2}", m.Hour, m.Minute) }), "Value", "Text", _date);
            return horarios;

        }

        public static string[] Lines(this string source)
        {
            return source.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static string encode(string text)
        {
            byte[] mybyte = System.Text.Encoding.UTF8.GetBytes(text);
            string returntext = System.Convert.ToBase64String(mybyte);
            return returntext;
        }

        public static string decode(string text)
        {
            byte[] mybyte = Convert.FromBase64String(text);
            string returntext = System.Text.Encoding.UTF8.GetString(mybyte);
            return returntext;
        }

        public static SelectList SelecionarSelectListUnitario<T>(IReadOnlyList<T> items, string id, string descricao, out long identificador)
        {
            identificador = 0;
            if (items.Count == 1)
            {
                var item = ((List<T>)items)[0];
                var i = item.GetType().GetProperty("Id").GetValue(item);

                long.TryParse(i.ToString(), out identificador);

                return new SelectList(items, id, descricao, identificador);
            }


            return new SelectList(items, id, descricao);
        }
        //public static string ReturnView(string viewName, object model)
        //{
        //    if (string.IsNullOrEmpty(viewName))
        //        viewName = ControllerContext.RouteData.GetRequiredString("action");

        //    ViewData.Model = model;

        //    using (StringWriter sw = new StringWriter())
        //    {
        //        ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
        //        ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}
    }
}