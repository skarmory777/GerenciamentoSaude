using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.SalasCirurgicas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Sessions;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.SalasCirurgicas.Servicos
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;
    using System.Data.Entity;

    public class SalaCirurgicaAppService : SWMANAGERAppServiceBase, ISalaCirurgicaAppService
    {

        private readonly ISessionAppService _sessionService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SalaCirurgicaAppService(ISessionAppService sessionService
                                            , IUnitOfWorkManager unitOfWorkManager)
        {
            _sessionService = sessionService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            var salaCirurgicaRepository = new SWRepository<SalaCirurgica>(AbpSession, _sessionService);

            return await this.CreateSelect2(salaCirurgicaRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        public async Task<List<SalaCirurgicaDto>> Listar()
        {

            var salaCirurgicaRepository = new SWRepository<SalaCirurgica>(AbpSession, _sessionService);

            var salas = await salaCirurgicaRepository.GetAll().ToListAsync().ConfigureAwait(false);

            return SalaCirurgicaDto.Mapear(salas);


        }

        public async Task<SalaCirurgicaDto> Obter(long id)
        {
            var salaCirurgicaRepository = new SWRepository<SalaCirurgica>(AbpSession, _sessionService);

            var salaCirurgica = await salaCirurgicaRepository
                                                       .GetAll().AsNoTracking()
                                                       .FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);

            return SalaCirurgicaDto.Mapear(salaCirurgica);
        }
    }
}
