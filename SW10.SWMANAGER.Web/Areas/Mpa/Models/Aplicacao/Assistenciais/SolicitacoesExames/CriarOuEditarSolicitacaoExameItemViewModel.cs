using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.SolicitacoesExames
{
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;

    [AutoMap(typeof(SolicitacaoExameItemDto))]
    public class CriarOuEditarSolicitacaoExameItemViewModel : SolicitacaoExameItemDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode { get { return Id > 0; } }

        public AtendimentoDto Atendimento { get; set; }

        public CriarOuEditarSolicitacaoExameItemViewModel(SolicitacaoExameItemDto output)
        {
            output.MapTo(this);
        }
    }
}