using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface ISolicitacaoAntimicrobianoAppService : IApplicationService
    {

        Task<PagedResultDto<SolicitacaoAntimicrobianoDto>> Listar(ListarInput input);

        SolicitacaoAntimicrobianoListDto CriarSolicitacoesParaPreencherPorAtendimentoEPrescricaoId(long atendimentoId, long PrescricaoId);

        Task<SolicitacaoAntimicrobianoDto> ObterSolicitacaoPorId(long id);


        bool ValidaSolicitacaoAntimicrobiano(long atendimentoId, List<long> prescricaoItemIds);

        ValidacaoSolicitacaoDto ValidaSolicitacaoAntimicrobianoPorPrescricao(long atendimentoId, long prescricaoId);

        Task<ResultSolicitacaoAntimicrobianoDto> SalvarSolicitacoes(SolicitacaoAntimicrobianoListDto input);

        byte[] RetornaArquivoSolicitacaoAntimicrobiano(List<long> ids);

        Task<SolicitacaoAntimicrobianosViewModel> SolicitacaoAntimicrobianoModal(long atendimentoId, long? prescricaoId);
        
    }
}
