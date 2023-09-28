using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    public class TipoRespostaAppService : SWMANAGERAppServiceBase, ITipoRespostaAppService
    {
        [UnitOfWork]
        public async Task<TipoRespostaDto> CriarOuEditar(TipoRespostaDto input)
        {
            try
            {
                using (var _tipoRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoResposta, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var tipoResposta = input.MapTo<TipoResposta>();
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _tipoRespostaRepositorio.Object.InsertAndGetIdAsync(tipoResposta);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            tipoResposta = await _tipoRespostaRepositorio.Object.UpdateAsync(tipoResposta);
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
        public async Task Excluir(TipoRespostaDto input)
        {
            try
            {
                using (var _tipoRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoResposta, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    await _tipoRespostaRepositorio.Object.DeleteAsync(input.Id);
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
        public async Task<PagedResultDto<TipoRespostaDto>> Listar(ListarTipoRespostaInput input)
        {
            var contarTipoResposta = 0;
            List<TipoResposta> tipoResposta;
            List<TipoRespostaDto> TipoRespostaDtos = new List<TipoRespostaDto>();
            try
            {
                using (var _tipoRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoResposta, long>>())
                {
                    var query = _tipoRespostaRepositorio.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                    contarTipoResposta = await query
                        .CountAsync();

                    tipoResposta = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    TipoRespostaDtos = tipoResposta
                        .MapTo<List<TipoRespostaDto>>();

                    return new PagedResultDto<TipoRespostaDto>(
                        contarTipoResposta,
                        TipoRespostaDtos
                        );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<TipoRespostaDto> Obter(long id)
        {
            try
            {
                using (var _tipoRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoResposta, long>>())
                {
                    var result = await _tipoRespostaRepositorio.Object
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();
                    var tipoResposta = result.MapTo<TipoRespostaDto>();

                    return tipoResposta;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoRespostaDto>> ListarTodos()
        {
            try
            {
                using (var _tipoRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoResposta, long>>())
                {
                    var query = _tipoRespostaRepositorio.Object
                    .GetAll()
                    .OrderBy(m => m.Codigo);

                    var tipoResposta = await query
                        .AsNoTracking()
                        .ToListAsync();

                    var tiposRespostasDto = tipoResposta
                        .MapTo<List<TipoRespostaDto>>();

                    return new ListResultDto<TipoRespostaDto>
                    {
                        Items = tiposRespostasDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoRespostaDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var _tipoRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoResposta, long>>())
                {
                    var query = _tipoRespostaRepositorio.Object
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(filtro) ||
                        m.Descricao.Contains(filtro)
                        );

                    var tipoResposta = await query
                                           .AsNoTracking()
                                           .ToListAsync().ConfigureAwait(false);

                    var tiposRespostasDto = tipoResposta
                        .MapTo<List<TipoRespostaDto>>();

                    return new ListResultDto<TipoRespostaDto>
                    {
                        Items = tiposRespostasDto
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
            using (var _tipoRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoResposta, long>>())
                return await this.CreateSelect2(_tipoRespostaRepositorio.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [UnitOfWork(false)]
        public Task<FileDto> ListarParaExcel(ListarTipoRespostaInput input)
        {
            throw new NotImplementedException();
        }

    }
}
