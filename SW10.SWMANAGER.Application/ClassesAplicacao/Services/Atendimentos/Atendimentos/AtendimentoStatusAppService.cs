using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class AtendimentoStatusAppService : SWMANAGERAppServiceBase, IAtendimentoStatusAppService
    {
        public readonly IRepository<AtendimentoStatus, long> _atendimentoStatusRepository;

        public AtendimentoStatusAppService(IRepository<AtendimentoStatus, long> atendimentoStatusRepository)
        {
            _atendimentoStatusRepository = atendimentoStatusRepository;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(_atendimentoStatusRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }
}
