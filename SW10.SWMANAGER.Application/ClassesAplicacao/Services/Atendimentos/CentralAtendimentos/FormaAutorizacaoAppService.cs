using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class FormaAutorizacaoAppService : SWMANAGERAppServiceBase, IFormaAutorizacaoAppService
    {
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var formaAutorizacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAutorizacao, long>>())
            {
                return await this.CreateSelect2(formaAutorizacaoRepository.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }
    }
}
