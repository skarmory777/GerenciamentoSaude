using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.Operacoes
{
    [AutoMapFrom(typeof(OperacaoDto))]
    public class CriarOuEditarOperacaoModalViewModel : OperacaoDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }

        public CriarOuEditarOperacaoModalViewModel(OperacaoDto output)
        {
            output.MapTo(this);
        }
    }
}