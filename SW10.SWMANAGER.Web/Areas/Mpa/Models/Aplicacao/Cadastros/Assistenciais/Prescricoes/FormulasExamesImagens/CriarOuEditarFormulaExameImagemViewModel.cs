using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormulasExamesImagens
{
    [AutoMap(typeof(FormulaExameImagemDto))]
    public class CriarOuEditarFormulaExameImagemViewModel : FormulaExameImagemDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarFormulaExameImagemViewModel(FormulaExameImagemDto output)
        {
            output.MapTo(this);
        }
    }
}