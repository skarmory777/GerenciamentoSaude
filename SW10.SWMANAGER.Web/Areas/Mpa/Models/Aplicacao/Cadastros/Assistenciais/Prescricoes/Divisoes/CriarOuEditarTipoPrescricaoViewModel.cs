using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes
{
    [AutoMap(typeof(TipoPrescricaoDto))]
    public class CriarOuEditarTipoPrescricaoViewModel : TipoPrescricaoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarTipoPrescricaoViewModel(TipoPrescricaoDto output)
        {
            output.MapTo(this);
        }
    }
}