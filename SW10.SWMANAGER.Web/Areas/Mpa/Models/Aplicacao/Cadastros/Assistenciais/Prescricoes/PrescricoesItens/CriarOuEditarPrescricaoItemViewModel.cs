using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    [AutoMap(typeof(PrescricaoItemDto))]
    public class CriarOuEditarPrescricaoItemViewModel : PrescricaoItemDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public bool IsCadastroProduto { get; set; }

        public CriarOuEditarPrescricaoItemViewModel(PrescricaoItemDto output)
        {
            output.MapTo(this);
        }
    }
}