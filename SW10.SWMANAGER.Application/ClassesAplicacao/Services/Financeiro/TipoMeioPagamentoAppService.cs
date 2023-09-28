using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class TipoMeioPagamentoAppService : SWMANAGERAppServiceBase, ITipoMeioPagamentoAppService
    {
        private readonly IRepository<TipoMeioPagamento, long> _tipoMeioPagamentoRepository;

        public TipoMeioPagamentoAppService(IRepository<TipoMeioPagamento, long> tipoMeioPagamentoRepository)
        {
            _tipoMeioPagamentoRepository = tipoMeioPagamentoRepository;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._tipoMeioPagamentoRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        public async Task<TipoMeioPagamentoDto> Obter(long id)
        {
            try
            {
                var query = _tipoMeioPagamentoRepository
                    .GetAll()
                    .FirstOrDefault(m => m.Id == id);

                return query.MapTo<TipoMeioPagamentoDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
