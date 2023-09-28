using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SolicitacaoAutorizacoes
{
    public interface ISolicitacaoAutorizacaoAppService : IApplicationService
    {
        Task<PagedResultDto<SolicitacaoAutorizacaoDto>> Listar(ListarInput input);

        SolicitacaoAutorizacaoListDto CriarSolicitacoesParaPreencherPorAtendimentoEPrescricaoItems(long atendimentoId, List<long> PrescricaoItemIds);

        Task<SolicitacaoAutorizacaoDto> ObterSolicitacaoPorId(long id);


        bool ValidaSolicitacaoAutorizacao(long atendimentoId, List<long> prescricaoItemIds);

        Task<ValidacaoSolicitacaoDto> ValidaSolicitacaoAutorizacaoPorPrescricao(long atendimentoId, long prescricaoId);

        Task<ResultSolicitacaoAutorizacaoDto> SalvarSolicitacoes(SolicitacaoAutorizacaoListDto input);

        byte[] RetornaArquivoSolicitacaoAutorizacao(List<long> ids);

        Task<SolicitacaoAutorizacoesViewModel> SolicitacaoAutorizacaoModal(long atendimentoId, long? prescricaoId);
    }
}