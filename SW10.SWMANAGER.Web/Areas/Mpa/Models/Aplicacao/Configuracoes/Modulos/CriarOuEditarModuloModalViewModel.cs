using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.Modulos
{
    [AutoMapFrom(typeof(ModuloDto))]
    public class CriarOuEditarModuloModalViewModel : ModuloDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }

        public CriarOuEditarModuloModalViewModel(ModuloDto output)
        {
            output.MapTo(this);
        }
    }
}