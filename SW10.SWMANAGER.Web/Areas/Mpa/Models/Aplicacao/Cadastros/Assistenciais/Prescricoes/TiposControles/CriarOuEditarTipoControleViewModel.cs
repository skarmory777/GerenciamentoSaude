using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles
{
    [AutoMap(typeof(TipoControleDto))]
    public class CriarOuEditarTipoControleViewModel : TipoControleDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarTipoControleViewModel(TipoControleDto output)
        {
            output.MapTo(this);
        }
    }
}