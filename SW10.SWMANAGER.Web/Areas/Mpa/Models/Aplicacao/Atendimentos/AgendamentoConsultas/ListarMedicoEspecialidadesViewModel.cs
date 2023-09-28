using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;

using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    public class ListarMedicoEspecialidadesViewModel
    {
        public Medico Medico { get; set; }
        public string DiasSemana { get; set; }
        public List<AgendamentoViewModel> Agendamentos { get; set; }
    }
}