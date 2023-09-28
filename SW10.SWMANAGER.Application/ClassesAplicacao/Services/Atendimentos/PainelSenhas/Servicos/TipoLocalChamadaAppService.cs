using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Servicos
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class TipoLocalChamadaAppService : SWMANAGERAppServiceBase, ITipoLocalChamadaAppService
    {
        private readonly IRepository<TipoLocalChamada, long> _tipoLocalChamadaRepository;

        public TipoLocalChamadaAppService(IRepository<TipoLocalChamada, long> tipoLocalChamadaRepository)
        {
            _tipoLocalChamadaRepository = tipoLocalChamadaRepository;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarTipoLocalChamadaDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._tipoLocalChamadaRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
