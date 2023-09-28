using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormulasEstoques
{
    [AutoMap(typeof(FormulaEstoqueDto))]
    public class CriarOuEditarFormulaEstoqueViewModel : FormulaEstoqueDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarFormulaEstoqueViewModel(FormulaEstoqueDto output)
        {
            output.MapTo(this);
        }
    }
}