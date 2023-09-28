using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.OrganizationUnits;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.UnidadesOrganizacionais
{
    [AutoMapFrom(typeof(UnidadeOrganizacionalDto))]
    public class CriarOuEditarUnidadeOrganizacionalModalViewModel : UnidadeOrganizacionalDto
    {
        public UserEditDto UpdateUser { get; set; }

        public CreateOrganizationUnitModalViewModel CreateOrganizationUnit { get; set; }

        public string OrganizationUnitNome { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }

        public SelectList UnidadesOrganizacionais { get; internal set; }

        public CriarOuEditarUnidadeOrganizacionalModalViewModel(UnidadeOrganizacionalDto output)
        {
            output.MapTo(this);
        }
    }
}