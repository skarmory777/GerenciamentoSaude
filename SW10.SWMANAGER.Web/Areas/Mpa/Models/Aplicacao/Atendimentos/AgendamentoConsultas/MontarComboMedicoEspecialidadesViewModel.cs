using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    public class MontarComboMedicoEspecialidadesViewModel
    {
        public long MedicoEspecialidadeId { get; set; }
        public SelectList MedicoEspecialidades { get; set; }
    }
}