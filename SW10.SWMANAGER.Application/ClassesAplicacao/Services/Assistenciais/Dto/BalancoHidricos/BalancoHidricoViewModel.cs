// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Class1.cs" company="">
//   
// </copyright>
// <summary>
//   The balanco hidrico view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Internal;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.BalancoHidrico;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto.BalancoHidricos
{
    /// <summary>
    /// The balanco hidrico view model.
    /// </summary>
    public class BalancoHidricoViewModel
    {
        public static int NumSolucoes = 12;

        public long BalancoHidricoAnteriorId { get; set; }

        /// <summary>
        /// The is edit mode.
        /// </summary>
        public bool IsEditMode => this.Model.Id > 0;

        /// <summary>
        /// Gets or sets the criar ou editar paciente modal view model.
        /// </summary>
        public AtendimentoDto Atendimento { get; set; }

        /// <summary>
        /// Gets or sets the balanco date.
        /// </summary>
        public DateTime? BalancoDate { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public BalancoHidricoDto Model { get; set; }


        public DateTime? DateLinhaTransferencia { get; set; }
        public BalancoHidricoItemDto LinhaTransferencia { get; set; }
        
        public List<BalancoHidricoItemDto> LinhaTransferenciaDiaAtual { get; set; }

        /// <summary>
        /// Gets or sets the balanco hidrico 24 hrs.
        /// </summary>
        public BalancoHidrico24HrsViewModel BalancoHidrico24Hrs { get; set; }

        public BalancoHidricoDto BalancoHidrico24 { get; set; }

        private int BaseIndex = 0;

        public int RetornaIndex()
        {
            return BaseIndex++;
        }

        public static HtmlString ConferidoHtml(BalancoHidricoItemDto item, BalancoHidricoViewModel model)
        {
            if (IsConferido(item, model.Model) ||
                IsBloqueado(item, model.Model.DataBalancoHidrico, model.Atendimento.DataRegistro))
            {
                return new HtmlString(@"disabled=""disabled""");
            }

            return new HtmlString("");
        }

        public static bool IsConferido(BalancoHidricoItemDto item, BalancoHidricoDto model)
        {
            var noite = model.ConferidoNoite &&
                        (item.Hora < TimeSpan.FromHours(7) || item.Hora >= TimeSpan.FromHours(19));
            var manha = model.ConferidoManha &&
                        (item.Hora >= TimeSpan.FromHours(7) && item.Hora < TimeSpan.FromHours(19));
            return model.ConferidoTotal || noite || manha;
        }

        public static bool IsBloqueado(BalancoHidricoItemDto item, DateTime balancoDate, DateTime atendimentoData)
        {
            var previousDate = atendimentoData.Date.Add(TimeSpan.FromDays(-1));

            return atendimentoData.TimeOfDay >= TimeSpan.FromHours(0) &&
                   atendimentoData.TimeOfDay <= TimeSpan.FromHours(7) && balancoDate <= previousDate &&
                   (item.Hora >= TimeSpan.FromHours(7) || item.Hora <= TimeSpan.FromHours(0));
        }


        public static HtmlString CriarCampoBalanco(BalancoHidricoItemDto item, BalancoHidricoViewModel model, object valor, string id)
        {
            return CriarCampoBalanco(item, model, valor, id, "width: 100%");
        }

        public static HtmlString CriarCampoBalanco(BalancoHidricoItemDto item, BalancoHidricoViewModel model, object valor, string id, string style = "width: 100%")
        {
            var name = string.Format("{0}_{1}", id, item.Hora.Hours);
            var inputTemplate = @"<input name=""{0}"" value=""{1}"" {2} style=""{3}""  class=""navegavel"" data-index=""{4}"" data-field=""{5}"" />";
            return new HtmlString(string.Format(inputTemplate, name, valor, ConferidoHtml(item, model), style, model.RetornaIndex(), id));
        }

        public static HtmlString SinaisVitaisSumario(string id, string shortLabel, string label,
            IEnumerable<SinalVitalViewModel> sinaisVitaisDtos, bool hasBadage = false,
            LimitViewModel limitViewModel = null)
        {
            var templateBtn =
                @"<button type=""button"" class=""btn btn-transparent btn-grafico"" data-grafico=""{2}"" data-toggle=""tooltip"" data-placement=""top"" title=""Gráfico 24 hrs""><i class=""fas fa-chart-line""></i></button>";
            if (sinaisVitaisDtos.IsNullOrEmpty() || !sinaisVitaisDtos.Where(x => x.Number.HasValue).Any())
            {
                templateBtn = "<div>&nbsp;</div>";
            }

            var template = @"<div class=""sinais-vitais-summario-item {0}"">
                <div class=""label-div""> <span class=""sinal-label"" data-toggle=""tooltip"" data-placement=""top"" title=""{0}"">{1}</span> </div>
                <div class=""number-div""> {3} </div>
                " + templateBtn + @"
                <input type=""hidden"" value='{4}' id=""input_hidden_{2}""/>
            </div>";
            if (hasBadage)
            {
                return new HtmlString(string.Format(template, label, shortLabel, id, SinaisVitaisSumarioNumberBadage(sinaisVitaisDtos, limitViewModel), JsonConvert.SerializeObject(sinaisVitaisDtos)));
            }

            return new HtmlString(string.Format(template, label, shortLabel, id, SinaisVitaisSumarioNumber(sinaisVitaisDtos), JsonConvert.SerializeObject(sinaisVitaisDtos)));
        }

        public static string SinaisVitaisSumarioNumber(IEnumerable<SinalVitalViewModel> sinaisVitaisDtos)
        {
            if (sinaisVitaisDtos.IsNullOrEmpty())
            {
                return "";
            }

            var min = sinaisVitaisDtos.Where(x => x.Number.HasValue).Min();
            var max = sinaisVitaisDtos.Where(x => x.Number.HasValue).Max();
            var template =
                @"<span data-toggle=""tooltip"" data-placement=""top"" title=""{1}"" class=""{2}""  style=""cursor: pointer"">{0}</span>";
            var minTemplate = "";
            var maxTemplate = "";
            if (min == null || !min.Number.HasValue || min.Number.Value == 0)
            {
                minTemplate = @"<span  class="" min"" style=""cursor: pointer; min-width:30px"">&nbsp;</span>";
            }
            else
            {
                minTemplate = string.Format(template, min.Valor, min.Hora.ToString(@"hh\:mm"), "min");
            }

            if (max == null || !max.Number.HasValue || max.Number.Value == 0)
            {
                maxTemplate = @"<span  class="" min"" style=""cursor: pointer; min-width:30px"">&nbsp;</span>";
            }
            else
            {
                maxTemplate = string.Format(template, max.Valor, max.Hora.ToString(@"hh\:mm"), "max");
            }

            return minTemplate + " / " + maxTemplate;
        }

        public static string SinaisVitaisSumarioNumberBadage(IEnumerable<SinalVitalViewModel> sinaisVitaisDtos,
            LimitViewModel limitViewModel)
        {
            if (sinaisVitaisDtos.IsNullOrEmpty())
            {
                return "";
            }

            var min = sinaisVitaisDtos.Where(x => x.Number.HasValue).Min();
            var max = sinaisVitaisDtos.Where(x => x.Number.HasValue).Max();

            var template =
                "<span data-toggle=\"tooltip\" data-placement=\"top\" title=\" {0} \" class=\"badge {2}\"  style=\"cursor: pointer\"> {1} </span>";
            var minTemplate = "";
            var maxTemplate = "";
            var badge = "";
            if (min == null || !min.Number.HasValue || min.Number.Value == 0)
            {
                minTemplate = @"<span  class=""badge badge-secondary min"" style=""cursor: pointer"">&nbsp;</span>";
            }
            else
            {
                badge = "badge-secondary";
                if (limitViewModel != null && limitViewModel.GetDanger(min.Number))
                {
                    badge = "badge-danger";
                }

                if (limitViewModel != null && limitViewModel.GetWarning(min.Number))
                {
                    badge = "badge-warning";
                }

                minTemplate = string.Format(template, min.Hora.ToString(@"hh\:mm"), min.Valor, badge + " min");
            }

            if (max == null || !max.Number.HasValue || max.Number.Value == 0)
            {
                maxTemplate = @"<span  class=""badge badge-secondary max"" style=""cursor: pointer"">&nbsp;</span>";
            }
            else
            {
                badge = "badge-secondary";
                if (limitViewModel != null && limitViewModel.GetDanger(max.Number))
                {
                    badge = "badge-danger";
                }

                if (limitViewModel != null && limitViewModel.GetWarning(max.Number))
                {
                    badge = "badge-warning";
                }

                maxTemplate = string.Format(template, max.Hora.ToString(@"hh\:mm"), max.Valor, badge + " max");
            }

            return minTemplate + " / " + maxTemplate;
        }
    }
}