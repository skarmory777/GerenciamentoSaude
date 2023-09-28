using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    public class MontarComboQuantidadeHorariosViewModel
    {
        public SelectList QuantidadeHorarios { get; set; }
        public string QuantidadeHorario { get; set; }
    }
}