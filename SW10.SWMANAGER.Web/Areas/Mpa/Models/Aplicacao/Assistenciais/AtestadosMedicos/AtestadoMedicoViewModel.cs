using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.AtestadosMedicos
{
    public class AtestadoMedicoViewModel
    {
        public virtual ICollection<MailingTemplateDto> Templates { get; set; }

        public virtual PacienteDto Paciente { get; set; }

        public virtual MedicoDto Medico { get; set; }

        public virtual EmpresaDto Empresa { get; set; }
    }
}