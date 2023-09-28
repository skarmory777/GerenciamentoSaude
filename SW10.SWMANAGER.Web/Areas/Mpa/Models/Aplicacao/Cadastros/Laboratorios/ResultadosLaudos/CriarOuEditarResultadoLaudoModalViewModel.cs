using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.ResultadosLaudos
{
    [AutoMap(typeof(ResultadoLaudoDto))]
    public class CriarOuEditarResultadoLaudoModalViewModel : ResultadoLaudoDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarResultadoLaudoModalViewModel(ResultadoLaudoDto output)
        {
            output.MapTo(this);
        }
    }
}