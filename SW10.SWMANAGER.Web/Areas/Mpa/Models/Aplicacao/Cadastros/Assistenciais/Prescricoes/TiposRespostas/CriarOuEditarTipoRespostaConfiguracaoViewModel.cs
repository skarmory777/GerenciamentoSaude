using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    [AutoMap(typeof(TipoRespostaConfiguracaoDto))]
    public class CriarOuEditarTipoRespostaConfiguracaoViewModel : TipoRespostaConfiguracaoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarTipoRespostaConfiguracaoViewModel(TipoRespostaConfiguracaoDto output)
        {
            output.MapTo(this);
        }
    }
}