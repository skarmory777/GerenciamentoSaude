using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios
{
    public interface ILaboratorioPainelAppService : IApplicationService
    {
        Task<PagedResultDto<LaboratorioPainelIndexOutput>> RetornaPainelData(LaboratorioPainelIndexInput input);

        Task<LaboratorioPainelIndexCounters> RetornaContadores(LaboratorioPainelIndexInput input);

        Task<BuscarPorSolicitacaoDto> BuscarPorSolicitacao(string codigo);

        Task<PainelVerificaExamesDto> VerificaExamesBaixa(PainelVerificaExamesDto input);
    }
}