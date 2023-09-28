using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class TipoFreteAppService : SWMANAGERAppServiceBase, ITipoFreteAppService
    {
        private readonly IRepository<TipoFrete, long> _TipoFreteRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TipoFreteAppService(
         IRepository<TipoFrete, long> TipoFreteRepository,
         IUnitOfWorkManager unitOfWorkManager

         )
        {
            _unitOfWorkManager = unitOfWorkManager;
            _TipoFreteRepository = TipoFreteRepository;
        }



        public async Task<ListResultDto<TipoFreteDto>> ListarTodos()
        {
            var contarTipoFrete = 0;
            List<TipoFrete> TipoFretes;
            List<TipoFreteDto> TipoFreteDtos = new List<TipoFreteDto>();
            try
            {
                var query = _TipoFreteRepository
                    .GetAll();

                contarTipoFrete = await query
                                      .CountAsync().ConfigureAwait(false);

                TipoFretes = await query
                                 .AsNoTracking()
                                 .ToListAsync().ConfigureAwait(false);

                TipoFreteDtos = TipoFretes
                    .MapTo<List<TipoFreteDto>>();

                return new ListResultDto<TipoFreteDto> { Items = TipoFreteDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._TipoFreteRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        public async Task<TipoFreteDto> Obter(long id)
        {
            try
            {
                var tipoFrete = await this._TipoFreteRepository.GetAsync(id).ConfigureAwait(false);

                return tipoFrete.MapTo<TipoFreteDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


    }
}
