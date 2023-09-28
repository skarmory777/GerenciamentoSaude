using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus
{
    public class PrescricaoStatusAppService : SWMANAGERAppServiceBase, IPrescricaoStatusAppService
    {
        [UnitOfWork]
        public async Task<PrescricaoStatusDto> CriarOuEditar(PrescricaoStatusDto input)
        {
            try
            {
                using (var _prescricaoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoStatus, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var prescricaoStatus = PrescricaoStatusDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _prescricaoStatusRepositorio.Object.InsertAndGetIdAsync(prescricaoStatus).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            prescricaoStatus = await _prescricaoStatusRepositorio.Object.UpdateAsync(prescricaoStatus).ConfigureAwait(false);
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
        public async Task Excluir(PrescricaoStatusDto input)
        {
            try
            {
                using (var _prescricaoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoStatus, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                    {
                        await _prescricaoStatusRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<PrescricaoStatusDto>> Listar(ListarInput input)
        {
            var contarPrescricaoStatus = 0;
            List<PrescricaoStatus> prescricaoStatus;
            List<PrescricaoStatusDto> PrescricaoStatusDtos = new List<PrescricaoStatusDto>();
            try
            {
                using (var _prescricaoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoStatus, long>>())
                {
                    var query = _prescricaoStatusRepositorio.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                    contarPrescricaoStatus = await query
                                                 .CountAsync().ConfigureAwait(false);

                    prescricaoStatus = await query
                                           .AsNoTracking()
                                           .OrderBy(input.Sorting)
                                           .PageBy(input)
                                           .ToListAsync().ConfigureAwait(false);

                    PrescricaoStatusDtos = PrescricaoStatusDto.Mapear(prescricaoStatus).ToList();

                    return new PagedResultDto<PrescricaoStatusDto>(contarPrescricaoStatus, PrescricaoStatusDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PrescricaoStatusDto> Obter(long id)
        {
            try
            {
                using (var _prescricaoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoStatus, long>>())
                {
                    var result = await _prescricaoStatusRepositorio.Object
                                 .GetAll()
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                    var prescricaoStatus = PrescricaoStatusDto.Mapear(result);

                    return prescricaoStatus;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoStatusDto>> ListarTodos()
        {
            try
            {
                using (var _prescricaoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoStatus, long>>())
                {
                    var query = _prescricaoStatusRepositorio.Object
                    .GetAll()
                    .OrderBy(m => m.Codigo);

                    var prescricaoStatus = await query
                                               .AsNoTracking()
                                               .ToListAsync().ConfigureAwait(false);

                    var prescricoesStatusDto = PrescricaoStatusDto.Mapear(prescricaoStatus).ToList();

                    return new ListResultDto<PrescricaoStatusDto>
                    {
                        Items = prescricoesStatusDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoStatusDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var _prescricaoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoStatus, long>>())
                {
                    var query = _prescricaoStatusRepositorio.Object
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(filtro) ||
                        m.Descricao.Contains(filtro)
                        );

                    var prescricaoStatus = await query
                                               .AsNoTracking()
                                               .ToListAsync().ConfigureAwait(false);

                    var prescricoesStatusDto = PrescricaoStatusDto.Mapear(prescricaoStatus).ToList();

                    return new ListResultDto<PrescricaoStatusDto>
                    {
                        Items = prescricoesStatusDto
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
            using (var _prescricaoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoStatus, long>>())
                return await this.CreateSelect2(_prescricaoStatusRepositorio.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }
}
