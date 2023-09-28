using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles
{
    using Abp.Auditing;
    using Abp.Dependency;
    using SW10.SWMANAGER.Helpers;

    public class TipoControleAppService : SWMANAGERAppServiceBase, ITipoControleAppService
    {
        [UnitOfWork]
        public async Task<TipoControleDto> CriarOuEditar(TipoControleDto input)
        {
            try
            {
                using (var _tipoControleRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoControle, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var tipoControle = TipoControleDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _tipoControleRepositorio.Object.InsertAndGetIdAsync(tipoControle).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            tipoControle = await _tipoControleRepositorio.Object.UpdateAsync(tipoControle).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(TipoControleDto input)
        {
            try
            {
                using (var _tipoControleRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoControle, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    await _tipoControleRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<TipoControleDto>> Listar(ListarInput input)
        {
            var contarTipoControle = 0;
            List<TipoControle> tipoControle;
            List<TipoControleDto> TipoControleDtos = new List<TipoControleDto>();
            using (var _tipoControleRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoControle, long>>())
            {
                try
                {
                    var query = _tipoControleRepositorio.Object
                        .GetAll()
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                            m.Codigo.Contains(input.Filtro) ||
                            m.Descricao.Contains(input.Filtro)
                            );

                    contarTipoControle = await query.CountAsync().ConfigureAwait(false);

                    tipoControle = await query
                                       .AsNoTracking()
                                       .OrderBy(input.Sorting)
                                       .PageBy(input)
                                       .ToListAsync().ConfigureAwait(false);

                    TipoControleDtos = TipoControleDto.Mapear(tipoControle);

                    return new PagedResultDto<TipoControleDto>(contarTipoControle, TipoControleDtos);
                }
                catch (System.Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }

        [UnitOfWork(false)]
        public async Task<TipoControleDto> Obter(long id)
        {
            try
            {
                using (var _tipoControleRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoControle, long>>())
                {
                    var result = await _tipoControleRepositorio.Object
                                 .GetAll()
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                    var tipoControle = TipoControleDto.Mapear(result);

                    return tipoControle;
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoControleDto>> ListarTodos()
        {
            try
            {
                using (var _tipoControleRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoControle, long>>())
                {
                    var query = _tipoControleRepositorio.Object
                    .GetAll()
                    .OrderBy(m => m.Codigo);

                    var tipoControle = await query
                                           .AsNoTracking()
                                           .ToListAsync().ConfigureAwait(false);

                    var tiposControlesDto = TipoControleDto.Mapear(tipoControle);

                    return new ListResultDto<TipoControleDto>
                    {
                        Items = tiposControlesDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoControleDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var _tipoControleRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoControle, long>>())
                {
                    var query = _tipoControleRepositorio.Object
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(filtro) ||
                        m.Descricao.Contains(filtro)
                        );

                    var tipoControle = await query
                                           .AsNoTracking()
                                           .ToListAsync().ConfigureAwait(false);

                    var tiposControlesDto = TipoControleDto.Mapear(tipoControle);

                    return new ListResultDto<TipoControleDto>
                    {
                        Items = tiposControlesDto
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
            using (var _tipoControleRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoControle, long>>())
                return await this.CreateSelect2(_tipoControleRepositorio.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }
}
