using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus
{
    using Abp.Auditing;
    using Abp.Dependency;
    using SW10.SWMANAGER.Helpers;

    public class PrescricaoItemStatusAppService : SWMANAGERAppServiceBase, IPrescricaoItemStatusAppService
    {
        [UnitOfWork]
        public async Task<PrescricaoItemStatusDto> CriarOuEditar(PrescricaoItemStatusDto input)
        {
            try
            {
                using (var _prescricaoItemStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemStatus, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var prescricaoItemStatus = PrescricaoItemStatusDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _prescricaoItemStatusRepositorio.Object.InsertAndGetIdAsync(prescricaoItemStatus).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            prescricaoItemStatus = await _prescricaoItemStatusRepositorio.Object.UpdateAsync(prescricaoItemStatus).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(PrescricaoItemStatusDto input)
        {
            try
            {
                using (var _prescricaoItemStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemStatus, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    await _prescricaoItemStatusRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<PrescricaoItemStatusDto>> Listar(ListarInput input)
        {
            try
            {
                using (var _prescricaoItemStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemStatus, long>>())
                {
                    var query = _prescricaoItemStatusRepositorio.Object
                    .GetAll().AsNoTracking().WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                    var contarPrescricaoItemStatus = await query.CountAsync().ConfigureAwait(false);

                    var prescricaoItemStatus = await query
                                                   .OrderBy(input.Sorting)
                                                   .PageBy(input)
                                                   .ToListAsync().ConfigureAwait(false);

                    return new PagedResultDto<PrescricaoItemStatusDto>(contarPrescricaoItemStatus, PrescricaoItemStatusDto.Mapear(prescricaoItemStatus).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PrescricaoItemStatusDto> Obter(long id)
        {
            try
            {
                using (var _prescricaoItemStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemStatus, long>>())
                {
                    var result = await _prescricaoItemStatusRepositorio.Object
                                 .GetAll()
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                    var prescricaoItemStatus = PrescricaoItemStatusDto.Mapear(result);

                    return prescricaoItemStatus;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoItemStatusDto>> ListarTodos()
        {
            try
            {
                using (var _prescricaoItemStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemStatus, long>>())
                {
                    var query = _prescricaoItemStatusRepositorio.Object
                    .GetAll()
                    .AsNoTracking()
                    .OrderBy(m => m.Codigo);

                    var prescricaoItemStatus = await query.ToListAsync().ConfigureAwait(false);

                    return new ListResultDto<PrescricaoItemStatusDto>
                    {
                        Items = PrescricaoItemStatusDto.Mapear(prescricaoItemStatus).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoItemStatusDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var _prescricaoItemStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemStatus, long>>())
                {
                    var query = _prescricaoItemStatusRepositorio.Object
                    .GetAll().AsNoTracking()
                    .WhereIf(!filtro.IsNullOrEmpty(), m => m.Codigo.Contains(filtro) || m.Descricao.Contains(filtro));

                    var prescricaoItemStatus = await query.ToListAsync().ConfigureAwait(false);
                    return new ListResultDto<PrescricaoItemStatusDto>
                    {
                        Items = PrescricaoItemStatusDto.Mapear(prescricaoItemStatus).ToList()
                    };
                }
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
            using (var _prescricaoItemStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemStatus, long>>())
            {
                return await this.CreateSelect2(_prescricaoItemStatusRepositorio.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

    }
}
