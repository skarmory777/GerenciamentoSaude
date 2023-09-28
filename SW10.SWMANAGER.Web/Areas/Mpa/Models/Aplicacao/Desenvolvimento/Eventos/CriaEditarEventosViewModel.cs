using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.Eventos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Desenvolvimento.Eventos
{
    [AutoMap(typeof(EventoDto))]
    public class CriaEditarEventosViewModel : EventoDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriaEditarEventosViewModel(EventoDto output)
        {
            output.MapTo(this);
        }
    }
}