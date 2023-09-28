using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Intervalos
{
    [AutoMapFrom(typeof(CriarOuEditarIntervalo))]
    public class CriarOuEditarIntervaloModalViewModel : CriarOuEditarIntervalo
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarIntervaloModalViewModel(CriarOuEditarIntervalo output)
        {
            output.MapTo(this);
        }
    }
}