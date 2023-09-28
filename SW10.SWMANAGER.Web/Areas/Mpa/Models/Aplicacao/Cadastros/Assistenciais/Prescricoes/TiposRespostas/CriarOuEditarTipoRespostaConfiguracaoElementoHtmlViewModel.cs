using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    [AutoMap(typeof(TipoRespostaConfiguracaoElementoHtmlDto))]
    public class CriarOuEditarTipoRespostaConfiguracaoElementoHtmlViewModel : TipoRespostaConfiguracaoElementoHtmlDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarTipoRespostaConfiguracaoElementoHtmlViewModel(TipoRespostaConfiguracaoElementoHtmlDto output)
        {
            output.MapTo(this);
        }
    }
}