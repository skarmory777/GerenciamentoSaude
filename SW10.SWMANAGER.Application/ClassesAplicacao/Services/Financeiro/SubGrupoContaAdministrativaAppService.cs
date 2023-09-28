using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class SubGrupoContaAdministrativaAppService : SWMANAGERAppServiceBase, ISubGrupoContaAdministrativaAppService
    {
        private readonly IRepository<SubGrupoContaAdministrativa, long> _subGrupoContaAdministrativaRepository;

        public SubGrupoContaAdministrativaAppService(IRepository<SubGrupoContaAdministrativa, long> subGrupoContaAdministrativaRepository)
        {
            _subGrupoContaAdministrativaRepository = subGrupoContaAdministrativaRepository;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._subGrupoContaAdministrativaRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
