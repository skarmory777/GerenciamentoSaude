using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.VersoesTiss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class VersaoTissAppService : SWMANAGERAppServiceBase, IVersaoTissAppService
    {
        private readonly IRepository<VersaoTiss, long> _versaoTissRepository;
        private readonly IListarVersoesTissExcelExporter _listarVersaoTissExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public VersaoTissAppService(IRepository<VersaoTiss, long> versaoTissRepository,
            IListarVersoesTissExcelExporter listarVersaoTissExcelExporter,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _versaoTissRepository = versaoTissRepository;
            _listarVersaoTissExcelExporter = listarVersaoTissExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;

        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Edit)]
        public async Task CriarOuEditar(CriarOuEditarVersaoTiss input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    var versaoTiss = input.MapTo<VersaoTiss>();
                    if (input.Id.Equals(0))
                    {
                        await this._versaoTissRepository.InsertAsync(versaoTiss).ConfigureAwait(false);
                    }
                    else
                    {
                        await this._versaoTissRepository.UpdateAsync(versaoTiss).ConfigureAwait(false);
                    }
                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarVersaoTiss input)
        {
            try
            {
                await this._versaoTissRepository.DeleteAsync(input.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<VersaoTissDto>> ListarTodos()
        {
            try
            {
                var query = await this._versaoTissRepository
                                .GetAllListAsync().ConfigureAwait(false);

                var versoesTissDto = query.MapTo<List<VersaoTissDto>>();

                return new ListResultDto<VersaoTissDto> { Items = versoesTissDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<VersaoTissDto>> Listar(ListarVersoesTissInput input)
        {
            var contarVersoesTiss = 0;
            List<VersaoTiss> versoesTiss;
            List<VersaoTissDto> versoesTissDtos = new List<VersaoTissDto>();
            try
            {
                var query = _versaoTissRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarVersoesTiss = await query
                                        .CountAsync().ConfigureAwait(false);

                versoesTiss = await query
                                  .AsNoTracking()
                                  .OrderBy(input.Sorting)
                                  .PageBy(input)
                                  .ToListAsync().ConfigureAwait(false);

                versoesTissDtos = versoesTiss
                    .MapTo<List<VersaoTissDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<VersaoTissDto>(
                contarVersoesTiss,
                versoesTissDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarVersoesTissInput input)
        {
            try
            {
                var query = await this.Listar(input).ConfigureAwait(false);

                var versoesTissDtos = query.Items;

                return _listarVersaoTissExcelExporter.ExportToFile(versoesTissDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarVersaoTiss> Obter(long id)
        {
            try
            {
                var result = await this._versaoTissRepository.GetAsync(id).ConfigureAwait(false);
                var versaoTiss = result.MapTo<CriarOuEditarVersaoTiss>();
                return versaoTiss;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ICollection<VersaoTissDto>> ListarPorTabelaDominio(long id)
        {
            List<VersaoTiss> versoesTiss;
            List<VersaoTissDto> versoesTissDtos = new List<VersaoTissDto>();
            try
            {
                var query = from m in _versaoTissRepository.GetAll()
                            from e in m.TabelaDominioVersoesTiss
                            where e.TabelaDominioId == id
                            select m;

                versoesTiss = await query
                                  .AsNoTracking()
                                  .ToListAsync().ConfigureAwait(false);

                versoesTissDtos = versoesTiss
                    .MapTo<List<VersaoTissDto>>();

                return versoesTissDtos;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            //return especialidadesDtos.MapTo<ListResultDto<VersaoTissDto>>();
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._versaoTissRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }
}