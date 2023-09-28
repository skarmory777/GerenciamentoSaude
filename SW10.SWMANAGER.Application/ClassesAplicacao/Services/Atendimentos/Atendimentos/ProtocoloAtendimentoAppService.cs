using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos
{
    using Abp.Auditing;
    using Abp.Domain.Uow;
    using SW10.SWMANAGER.Helpers;

    public class ProtocoloAtendimentoAppService : SWMANAGERAppServiceBase, IProtocoloAtendimentoAppService
    {
        public readonly IRepository<ProtocoloAtendimento, long> _protocoloAtendimentoRepository;

        public ProtocoloAtendimentoAppService(IRepository<ProtocoloAtendimento, long> protocoloAtendimentoRepository)
        {
            _protocoloAtendimentoRepository = protocoloAtendimentoRepository;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(_protocoloAtendimentoRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
