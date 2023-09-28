using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposLogradouro
{

    [AutoMapFrom(typeof(CriarOuEditarTipoLogradouroDto))]
    public class CriarOuEditarTiposLogradouroModalViewModel : CriarOuEditarTipoLogradouroDto
    {
        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarTiposLogradouroModalViewModel(CriarOuEditarTipoLogradouroDto output)
        {
            output.MapTo(this);
        }
    }
}