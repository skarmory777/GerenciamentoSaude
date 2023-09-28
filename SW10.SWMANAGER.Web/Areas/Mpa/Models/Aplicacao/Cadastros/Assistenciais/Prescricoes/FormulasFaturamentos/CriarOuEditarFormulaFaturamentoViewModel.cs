using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormulasFaturamentos
{
    [AutoMap(typeof(FormulaFaturamentoDto))]
    public class CriarOuEditarFormulaFaturamentoViewModel : FormulaFaturamentoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarFormulaFaturamentoViewModel(FormulaFaturamentoDto output)
        {
            output.MapTo(this);
        }
    }
}