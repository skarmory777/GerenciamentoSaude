using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos
{
    public class StatusSolicitacaoProcedimentoAppService : IStatusSolicitacaoProcedimentoAppService, IApplicationService
    {
        private readonly IRepository<StatusSolicitacaoProcedimento, long> _statusSolicitacaoProcedimentoRepository;

        public StatusSolicitacaoProcedimentoAppService(IRepository<StatusSolicitacaoProcedimento, long> statusSolicitacaoProcedimentoRepository)
        {
            _statusSolicitacaoProcedimentoRepository = statusSolicitacaoProcedimentoRepository;
        }



        public async Task<ListResultDto<StatusSolicitacaoProcedimentoDto>> ListarTodos()
        {
            var status = (await _statusSolicitacaoProcedimentoRepository
                .GetAllListAsync().ConfigureAwait(false))
                .Select(s => StatusSolicitacaoProcedimentoDto.Mapear(s))
                .ToList();


            //var statusSolicitacaoProcedimentoDto = status
            //    .MapTo<List<StatusSolicitacaoProcedimentoDto>>();

            return new ListResultDto<StatusSolicitacaoProcedimentoDto> { Items = status };

        }
    }
}
