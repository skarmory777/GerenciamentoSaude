using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormulasExamesLaboratoriais
{
    [AutoMap(typeof(FormulaExameLaboratorialDto))]
    public class CriarOuEditarFormulaExameLaboratorialViewModel : FormulaExameLaboratorialDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarFormulaExameLaboratorialViewModel(FormulaExameLaboratorialDto output)
        {
            output.MapTo(this);
        }
    }
}