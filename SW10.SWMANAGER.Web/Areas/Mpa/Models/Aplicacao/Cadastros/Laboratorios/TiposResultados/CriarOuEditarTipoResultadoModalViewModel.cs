using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.TiposResultados
{
    [AutoMap(typeof(TipoResultadoDto))]
    public class CriarOuEditarTipoResultadoModalViewModel : TipoResultadoDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarTipoResultadoModalViewModel(TipoResultadoDto output)
        {
            output.MapTo(this);
        }
    }
}