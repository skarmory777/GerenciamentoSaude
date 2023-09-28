using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GruposCID
{
    [AutoMapFrom(typeof(GrupoCIDDto))]
    public class CriarOuEditarGrupoCIDModalViewModel : GrupoCIDDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarGrupoCIDModalViewModel(GrupoCIDDto output)
        {
            output.MapTo(this);
        }
    }
}