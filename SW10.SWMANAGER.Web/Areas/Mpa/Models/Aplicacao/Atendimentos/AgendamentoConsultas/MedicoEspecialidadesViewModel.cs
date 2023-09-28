using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    public class MedicoEspecialidadesViewModel
    {
        public long MedicoEspecialidadeId { get; set; }

        public string NomeCombo { get; set; }

        public SelectList MedicoEspecialidades { get; set; }
    }
}