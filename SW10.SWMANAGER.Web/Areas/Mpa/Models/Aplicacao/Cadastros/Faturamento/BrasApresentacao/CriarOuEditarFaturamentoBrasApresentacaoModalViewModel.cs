using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasApresentacoes
{
    [AutoMapFrom(typeof(FaturamentoBrasApresentacaoDto))]
    public class CriarOuEditarFaturamentoBrasApresentacaoModalViewModel : FaturamentoBrasApresentacaoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoBrasApresentacaoModalViewModel(FaturamentoBrasApresentacaoDto output)
        {
            output.MapTo(this);
        }
    }
}