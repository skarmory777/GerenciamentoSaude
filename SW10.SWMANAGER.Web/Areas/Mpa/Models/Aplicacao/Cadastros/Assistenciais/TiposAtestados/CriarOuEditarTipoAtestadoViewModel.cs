using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.TiposAtestados
{
    [AutoMap(typeof(TipoAtestadoDto))]
    public class CriarOuEditarTipoAtestadoViewModel : TipoAtestadoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarTipoAtestadoViewModel(TipoAtestadoDto output)
        {
            output.MapTo(this);
        }
    }
}