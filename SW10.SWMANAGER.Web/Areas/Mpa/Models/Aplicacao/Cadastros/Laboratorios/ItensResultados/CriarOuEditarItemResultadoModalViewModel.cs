using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.ItensResultados
{
    [AutoMap(typeof(ItemResultadoDto))]
    public class CriarOuEditarItemResultadoModalViewModel : ItemResultadoDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarItemResultadoModalViewModel(ItemResultadoDto output)
        {
            output.MapTo(this);
        }
    }
}