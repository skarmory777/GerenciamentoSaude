using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    [AutoMap(typeof(TipoRespostaTipoRespostaConfiguracaoDto))]
    public class CriarOuEditarTipoRespostaTipoRespostaConfiguracaoViewModel : TipoRespostaTipoRespostaConfiguracaoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarTipoRespostaTipoRespostaConfiguracaoViewModel(TipoRespostaTipoRespostaConfiguracaoDto output)
        {
            output.MapTo(this);
        }
    }
}