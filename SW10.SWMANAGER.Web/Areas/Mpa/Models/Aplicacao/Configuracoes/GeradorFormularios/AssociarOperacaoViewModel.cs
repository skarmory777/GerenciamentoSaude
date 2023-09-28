using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.GeradorFormularios
{
    [AutoMap(typeof(FormConfigOperacaoDto))]
    public class AssociarOperacaoViewModel : FormConfigOperacaoDto
    {
        public string Filtro { get; set; }

        public long? ModuloId { get; set; }

        public ModuloDto Modulo { get; set; }

        public AssociarOperacaoViewModel(FormConfigOperacaoDto output)
        {
            output.MapTo(this);
        }
    }
}