using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    [AutoMap(typeof(TipoRespostaDto))]
    public class CriarOuEditarTipoRespostaViewModel : TipoRespostaDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarTipoRespostaViewModel(TipoRespostaDto output)
        {
            output.MapTo(this);
        }
    }
}