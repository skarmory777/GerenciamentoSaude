using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Diagnosticos.ModelosLaudos
{
    [AutoMap(typeof(ModeloLaudoDto))]
    public class CriarOuEditarModeloLaudoViewModel : ModeloLaudoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarModeloLaudoViewModel(ModeloLaudoDto output)
        {
            output.MapTo(this);
        }
    }
}