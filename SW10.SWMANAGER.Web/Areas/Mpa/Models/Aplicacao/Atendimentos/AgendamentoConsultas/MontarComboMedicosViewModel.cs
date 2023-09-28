using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    public class MontarComboMedicosViewModel
    {
        public long MedicoId { get; set; }
        public SelectList Medicos { get; set; }
    }
}