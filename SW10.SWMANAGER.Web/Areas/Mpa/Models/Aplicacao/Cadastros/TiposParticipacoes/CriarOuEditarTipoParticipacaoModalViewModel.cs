using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposParticipacoes
{
    [AutoMap(typeof(TipoParticipacaoDto))]
    public class CriarOuEditarTipoParticipacaoModalViewModel : TipoParticipacaoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarTipoParticipacaoModalViewModel(TipoParticipacaoDto output)
        {
            output.MapTo(this);
        }
    }
}