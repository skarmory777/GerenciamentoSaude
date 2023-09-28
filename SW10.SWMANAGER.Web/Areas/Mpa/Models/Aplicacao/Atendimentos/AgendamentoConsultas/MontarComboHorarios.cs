using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    public class MontarComboHorariosViewModel
    {
        public long AgendamentoConsultaMedicoDisponibilidadeId { get; set; }
        public SelectList Horarios { get; set; }
        public string Horario { get; set; }
    }
}