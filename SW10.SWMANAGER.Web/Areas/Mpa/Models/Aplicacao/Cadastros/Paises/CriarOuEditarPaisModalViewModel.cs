using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Paises
{
    [AutoMapFrom(typeof(CriarOuEditarPais))]
    public class CriarOuEditarPaisModalViewModel : CriarOuEditarPais
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarPaisModalViewModel(CriarOuEditarPais output)
        {
            output.MapTo(this);
        }
    }
}