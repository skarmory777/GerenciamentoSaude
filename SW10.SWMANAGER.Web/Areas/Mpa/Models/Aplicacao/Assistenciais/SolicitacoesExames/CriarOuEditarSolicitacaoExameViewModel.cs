using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.SolicitacoesExames
{
    [AutoMap(typeof(SolicitacaoExameDto))]
    public class CriarOuEditarSolicitacaoExameViewModel : SolicitacaoExameDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode { get { return Id > 0; } }

        public bool DataFutura { get; set; }

        public CriarOuEditarSolicitacaoExameViewModel(SolicitacaoExameDto output)
        {
            output.MapTo(this);
        }
    }
}