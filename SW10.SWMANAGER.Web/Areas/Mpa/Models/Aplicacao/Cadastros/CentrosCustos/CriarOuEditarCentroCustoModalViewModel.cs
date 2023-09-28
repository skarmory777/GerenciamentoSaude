using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.CentrosCustos
{
    [AutoMapFrom(typeof(CriarOuEditarCentroCusto))]
    public class CriarOuEditarCentroCustoModalViewModel : CriarOuEditarCentroCusto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }

        //public SelectList GruposCentroCustos { get; internal set; }
        //public SelectList UnidadesOrganizacionais { get; internal set; }

        public CriarOuEditarGrupoCentroCusto GrupoCentroCusto { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        public CriarOuEditarCentroCustoModalViewModel(CriarOuEditarCentroCusto output)
        {
            output.MapTo(this);
        }
    }
}