using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Nacionalidades
{
    [AutoMapFrom(typeof(CriarOuEditarNacionalidade))]
    public class CriarOuEditarNacionalidadeModalViewModel : CriarOuEditarNacionalidade
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarNacionalidadeModalViewModel(CriarOuEditarNacionalidade output)
        {
            output.MapTo(this);
        }
    }
}