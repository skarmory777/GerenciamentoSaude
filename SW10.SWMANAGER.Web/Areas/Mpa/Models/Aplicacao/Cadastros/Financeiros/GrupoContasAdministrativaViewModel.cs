using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros
{
    [AutoMap(typeof(GrupoContaAdministrativaDto))]
    public class GrupoContasAdministrativaViewModel : GrupoContaAdministrativaDto
    {
        public GrupoContasAdministrativaViewModel(GrupoContaAdministrativaDto output)
        {
            output.MapTo(this);
        }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public string Filtro { get; set; }
    }
}