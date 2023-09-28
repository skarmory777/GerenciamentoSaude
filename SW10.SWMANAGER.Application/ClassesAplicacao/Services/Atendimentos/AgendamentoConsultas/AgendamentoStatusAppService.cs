using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Sessions;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class AgendamentoStatusAppService : SWMANAGERAppServiceBase, IAgendamentoStatusAppService
    {

        private readonly ISessionAppService _sessionService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AgendamentoStatusAppService(ISessionAppService sessionService, IUnitOfWorkManager unitOfWorkManager)
        {
            _sessionService = sessionService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            var agendamentoStatusRepository = new SWRepository<AgendamentoStatus>(AbpSession, _sessionService);

            return await this.CreateSelect2(agendamentoStatusRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
