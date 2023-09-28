using Castle.Core.Internal;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Home;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Solicitacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SolicitacaoAutorizacoes;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Prescricoes
{
    public class SolicitacoesViewModel
    {
        public AssistenciaisViewModel HeaderPaciente { get; set; }

        public bool PossuiSolicitacaoAntimicrobiano => SolicitacaoAntimicrobianos != null && !SolicitacaoAntimicrobianos.SolicitacaoAntimicrobianos.IsNullOrEmpty();
        public SolicitacaoAntimicrobianosViewModel SolicitacaoAntimicrobianos { get; set; }

        public bool PossuiSolicitacaoAutorizacoes => SolicitacaoAutorizacoes != null && !SolicitacaoAutorizacoes.SolicitacaoAutorizacoes.IsNullOrEmpty();

        public SolicitacaoAutorizacoesViewModel SolicitacaoAutorizacoes { get; set; }
    }
}