using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Regioes
{
    [AutoMapFrom(typeof(CriarOuEditarRegiao))]
    public class CriarOuEditarRegiaoModalViewModel : CriarOuEditarRegiao
    {
        //public RegiaoDto Regiao { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }


        public CriarOuEditarRegiaoModalViewModel(CriarOuEditarRegiao output)
        {
            output.MapTo(this);
        }
    }
}