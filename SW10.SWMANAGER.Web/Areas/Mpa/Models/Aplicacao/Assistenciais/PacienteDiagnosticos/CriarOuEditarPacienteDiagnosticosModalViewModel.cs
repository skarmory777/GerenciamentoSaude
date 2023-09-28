using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.PacienteDiagnosticos
{
    [AutoMapFrom(typeof(PacienteDiagnosticosDto))]
    public class CriarOuEditarPacienteDiagnosticosModalViewModel : PacienteDiagnosticosDto
    {
        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarPacienteDiagnosticosModalViewModel(PacienteDiagnosticosDto output)
        {
            output.MapTo(this);
        }
    }
}