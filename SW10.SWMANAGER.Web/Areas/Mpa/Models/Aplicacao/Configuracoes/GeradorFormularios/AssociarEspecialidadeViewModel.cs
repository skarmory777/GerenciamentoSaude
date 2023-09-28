using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.GeradorFormularios
{
    [AutoMap(typeof(FormConfigEspecialidadeDto))]
    public class AssociarEspecialidadeViewModel : FormConfigEspecialidadeDto
    {
        public string Filtro { get; set; }

        //public long? ModuloId { get; set; }

        //public ModuloDto Modulo { get; set; }

        public AssociarEspecialidadeViewModel(FormConfigEspecialidadeDto output)
        {
            output.MapTo(this);
        }
    }
}