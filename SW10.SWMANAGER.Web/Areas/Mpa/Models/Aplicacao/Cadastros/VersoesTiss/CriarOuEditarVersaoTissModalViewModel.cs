using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.VersoesTiss
{
    [AutoMapFrom(typeof(CriarOuEditarVersaoTiss))]
    public class CriarOuEditarVersaoTissModalViewModel : CriarOuEditarVersaoTiss
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarVersaoTissModalViewModel(CriarOuEditarVersaoTiss output)
        {
            output.MapTo(this);
        }
    }
}