using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposUnidade
{
    [AutoMapFrom(typeof(CriarOuEditarTipoUnidade))]
    public class CriarOuEditarTipoUnidadeModalViewModel : CriarOuEditarTipoUnidade
    {
        //public TipoUnidadeDto TipoUnidade { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }


        public CriarOuEditarTipoUnidadeModalViewModel(CriarOuEditarTipoUnidade output)
        {
            output.MapTo(this);
        }
    }
}