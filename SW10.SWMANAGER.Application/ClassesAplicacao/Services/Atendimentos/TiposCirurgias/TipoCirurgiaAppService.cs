using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Sessions;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.TiposCirurgias
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;
    using System.Data.Entity;

    public class TipoCirurgiaAppService : SWMANAGERAppServiceBase, ITipoCirurgiaAppService
    {
        private readonly ISessionAppService _sessionService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TipoCirurgiaAppService(ISessionAppService sessionService
                                    , IUnitOfWorkManager unitOfWorkManager)
        {
            _sessionService = sessionService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            var salaCirurgicaRepository = new SWRepository<TipoCirurgia>(AbpSession, _sessionService);

            return await this.CreateSelect2(salaCirurgicaRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        public async Task<TipoCirurgiaDto> Obter(long id)
        {
            var salaCirurgicaRepository = new SWRepository<TipoCirurgia>(AbpSession, _sessionService);
            var query = await salaCirurgicaRepository
                                               .GetAll()
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);

            return TipoCirurgiaDto.Mapear(query);
        }
    }
}
