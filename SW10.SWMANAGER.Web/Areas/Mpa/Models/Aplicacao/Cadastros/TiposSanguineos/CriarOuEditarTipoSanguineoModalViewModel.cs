using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposSanguineos
{
    [AutoMap(typeof(TipoSanguineoDto))]
    public class CriarOuEditarTipoSanguineoModalViewModel : TipoSanguineoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarTipoSanguineoModalViewModel(TipoSanguineoDto output)
        {
            output.MapTo(this);
        }
    }
}