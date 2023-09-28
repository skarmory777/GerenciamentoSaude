using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Mapas
{
    [AutoMap(typeof(MapaDto))]
    public class CriarOuEditarMapaModalViewModel : MapaDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarMapaModalViewModel(MapaDto output)
        {
            output.MapTo(this);
        }
    }
}