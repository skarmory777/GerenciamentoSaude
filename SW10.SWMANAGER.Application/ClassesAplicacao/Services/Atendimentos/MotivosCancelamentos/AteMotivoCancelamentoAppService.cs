using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MotivosCancelamentos
{
    using Abp.Auditing;

    public class AteMotivoCancelamentoAppService : SWMANAGERAppServiceBase, IAteMotivoCancelamentoAppService
    {
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2<AtendimentoMotivoCancelamento,long>().ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
