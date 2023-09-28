using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacoes
{
    [AutoMap(typeof(FormaAplicacaoDto))]
    public class CriarOuEditarFormaAplicacaoViewModel : FormaAplicacaoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarFormaAplicacaoViewModel(FormaAplicacaoDto output)
        {
            output.MapTo(this);
        }
    }
}