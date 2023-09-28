using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.PacienteAlergias
{
    [AutoMapFrom(typeof(PacienteAlergiasDto))]
    public class CriarOuEditarPacienteAlergiasModalViewModel : PacienteAlergiasDto
    {
        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarPacienteAlergiasModalViewModel(PacienteAlergiasDto output)
        {
            output.MapTo(this);
        }
    }
}