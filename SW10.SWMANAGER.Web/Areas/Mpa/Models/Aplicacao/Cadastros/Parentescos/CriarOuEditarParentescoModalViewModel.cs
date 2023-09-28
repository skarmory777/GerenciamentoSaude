using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Parentescos
{
    [AutoMapFrom(typeof(CriarOuEditarParentesco))]
    public class CriarOuEditarParentescoModalViewModel : CriarOuEditarParentesco
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarParentescoModalViewModel(CriarOuEditarParentesco output)
        {
            output.MapTo(this);
        }
    }
}