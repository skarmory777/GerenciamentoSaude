using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.PacienteAlergias
{
    public class PacienteAlergiasViewModel
    {
        public IEnumerable<PacienteAlergiasDto> Alergias { get; set; }

        public PacienteDto Paciente { get; set; }

        public AtendimentoDto Atendimento { get; set; }
    }
}