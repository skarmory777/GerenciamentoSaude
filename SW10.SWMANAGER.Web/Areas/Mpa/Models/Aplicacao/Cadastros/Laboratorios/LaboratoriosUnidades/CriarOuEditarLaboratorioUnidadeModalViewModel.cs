using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.LaboratoriosUnidades
{
    [AutoMap(typeof(LaboratorioUnidadeDto))]
    public class CriarOuEditarLaboratorioUnidadeModalViewModel : LaboratorioUnidadeDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarLaboratorioUnidadeModalViewModel(LaboratorioUnidadeDto output)
        {
            output.MapTo(this);
        }
    }
}