using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.ResultadosExames
{
    [AutoMap(typeof(ResultadoExameDto))]
    public class CriarOuEditarResultadoExameModalViewModel : ResultadoExameDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarResultadoExameModalViewModel(ResultadoExameDto output)
        {
            output.MapTo(this);
        }
    }
}