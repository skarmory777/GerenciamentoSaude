using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos
{
    using Abp.Auditing;
    using Abp.Domain.Uow;
    using SW10.SWMANAGER.Helpers;

    public class ModalidadeAppService : SWMANAGERAppServiceBase, IModalidadeAppService
    {
        private readonly IRepository<Modalidade, long> _modalidadeRepository;

        public ModalidadeAppService(IRepository<Modalidade, long> modalidadeRepository)
        {
            _modalidadeRepository = modalidadeRepository;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._modalidadeRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
