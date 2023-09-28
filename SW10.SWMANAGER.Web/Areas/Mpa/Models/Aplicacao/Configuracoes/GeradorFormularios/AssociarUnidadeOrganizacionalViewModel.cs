using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.GeradorFormularios
{
    [AutoMap(typeof(FormConfigUnidadeOrganizacionalDto))]
    public class AssociarUnidadeOrganizacionalViewModel : FormConfigUnidadeOrganizacionalDto
    {
        public string Filtro { get; set; }

        public AssociarUnidadeOrganizacionalViewModel(FormConfigUnidadeOrganizacionalDto output)
        {
            output.MapTo(this);
        }
    }
}