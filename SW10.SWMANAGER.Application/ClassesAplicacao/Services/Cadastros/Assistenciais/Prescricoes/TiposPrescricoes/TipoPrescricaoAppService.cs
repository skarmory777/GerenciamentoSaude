using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes
{
    public class TipoPrescricaoAppService : SWMANAGERAppServiceBase, ITipoPrescricaoAppService
    {
        [UnitOfWork]
        public async Task<TipoPrescricaoDto> CriarOuEditar(TipoPrescricaoDto input)
        {
            try
            {
                using (var _tipoPrescricaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoPrescricao, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var tipoPrescricao = TipoPrescricaoDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {                        
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _tipoPrescricaoRepositorio.Object.InsertAndGetIdAsync(tipoPrescricao).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            tipoPrescricao = await _tipoPrescricaoRepositorio.Object.UpdateAsync(tipoPrescricao).ConfigureAwait(false);
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
        public async Task Excluir(TipoPrescricaoDto input)
        {
            try
            {
                using (var _tipoPrescricaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoPrescricao, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    await _tipoPrescricaoRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
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
        public async Task<PagedResultDto<TipoPrescricaoDto>> Listar(ListarInput input)
        {
            var contarTipoPrescricao = 0;
            List<TipoPrescricao> tipoPrescricao;
            List<TipoPrescricaoDto> TipoPrescricaoDtos = new List<TipoPrescricaoDto>();
            try
            {
                using (var _tipoPrescricaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoPrescricao, long>>())
                {
                    var query = _tipoPrescricaoRepositorio.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                    contarTipoPrescricao = await query
                                               .CountAsync().ConfigureAwait(false);

                    tipoPrescricao = await query
                                         .AsNoTracking()
                                         .OrderBy(input.Sorting)
                                         .PageBy(input)
                                         .ToListAsync().ConfigureAwait(false);

                    TipoPrescricaoDtos = TipoPrescricaoDto.Mapear(tipoPrescricao);

                    return new PagedResultDto<TipoPrescricaoDto>(contarTipoPrescricao, TipoPrescricaoDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<TipoPrescricaoDto> Obter(long id)
        {
            try
            {
                using (var _tipoPrescricaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoPrescricao, long>>())
                {
                    var result = await _tipoPrescricaoRepositorio.Object
                                 .GetAll()
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                    var tipoPrescricao = TipoPrescricaoDto.Mapear(result);

                    return tipoPrescricao;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoPrescricaoDto>> ListarTodos()
        {
            try
            {
                using (var _tipoPrescricaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoPrescricao, long>>())
                {
                    var query = _tipoPrescricaoRepositorio.Object
                    .GetAll()
                    .OrderBy(m => m.Codigo);

                    var tipoPrescricao = await query
                                             .AsNoTracking()
                                             .ToListAsync().ConfigureAwait(false);

                    var tiposPrescricoesDto = TipoPrescricaoDto.Mapear(tipoPrescricao);

                    return new ListResultDto<TipoPrescricaoDto>
                    {
                        Items = tiposPrescricoesDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoPrescricaoDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var _tipoPrescricaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoPrescricao, long>>())
                {
                    var query = _tipoPrescricaoRepositorio.Object
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(filtro) ||
                        m.Descricao.Contains(filtro)
                        );

                    var tipoPrescricao = await query
                                             .AsNoTracking()
                                             .ToListAsync().ConfigureAwait(false);

                    var tiposPrescricoesDto = TipoPrescricaoDto.Mapear(tipoPrescricao);

                    return new ListResultDto<TipoPrescricaoDto>
                    {
                        Items = tiposPrescricoesDto
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
            using (var _tipoPrescricaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoPrescricao, long>>())
                return await this.CreateSelect2(_tipoPrescricaoRepositorio.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }
}
