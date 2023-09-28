using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.CapitulosCID
{
    [AutoMap(typeof(CapituloCIDDto))]
    public class CriarOuEditarCapituloCIDModalViewModel : CapituloCIDDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarCapituloCIDModalViewModel(CapituloCIDDto output)
        {
            output.MapTo(this);
        }
    }
}