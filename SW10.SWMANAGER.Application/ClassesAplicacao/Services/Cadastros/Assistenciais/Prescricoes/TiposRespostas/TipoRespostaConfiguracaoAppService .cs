using Abp.Application.Services.Dto;
using Abp.Auditing;
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
    public class TipoRespostaConfiguracaoAppService : SWMANAGERAppServiceBase, ITipoRespostaConfiguracaoAppService
    {
        [UnitOfWork]
        public async Task<TipoRespostaConfiguracaoDto> CriarOuEditar(TipoRespostaConfiguracaoDto input)
        {
            try
            {
                using (var _tipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracao, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var tipoRespostaConfiguracao = TipoRespostaConfiguracaoDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _tipoRespostaConfiguracaoRepositorio.Object.InsertAndGetIdAsync(tipoRespostaConfiguracao);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            tipoRespostaConfiguracao = await _tipoRespostaConfiguracaoRepositorio.Object.UpdateAsync(tipoRespostaConfiguracao);
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
        public async Task Excluir(TipoRespostaConfiguracaoDto input)
        {
            try
            {
                using (var _tipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracao, long>>())
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    await _tipoRespostaConfiguracaoRepositorio.Object.DeleteAsync(input.Id);
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
        public async Task<PagedResultDto<TipoRespostaConfiguracaoDto>> Listar(ListarInput input)
        {
            var contarTipoRespostaConfiguracao = 0;
            List<TipoRespostaConfiguracao> tipoRespostaConfiguracao;
            List<TipoRespostaConfiguracaoDto> TipoRespostaConfiguracaoDtos = new List<TipoRespostaConfiguracaoDto>();
            try
            {
                using (var _tipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracao, long>>())
                {
                    var query = _tipoRespostaConfiguracaoRepositorio.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                    contarTipoRespostaConfiguracao = await query
                        .CountAsync();

                    tipoRespostaConfiguracao = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    TipoRespostaConfiguracaoDtos = TipoRespostaConfiguracaoDto.Mapear(tipoRespostaConfiguracao);

                    return new PagedResultDto<TipoRespostaConfiguracaoDto>(contarTipoRespostaConfiguracao, TipoRespostaConfiguracaoDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<TipoRespostaConfiguracaoDto> Obter(long id)
        {
            try
            {
                using (var _tipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracao, long>>())
                {
                    var result = await _tipoRespostaConfiguracaoRepositorio.Object
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();
                    var tipoRespostaConfiguracao = TipoRespostaConfiguracaoDto.Mapear(result);

                    return tipoRespostaConfiguracao;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoRespostaConfiguracaoDto>> ListarTodos()
        {
            try
            {
                using (var _tipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracao, long>>())
                {
                    var query = _tipoRespostaConfiguracaoRepositorio.Object.GetAll();

                    var tipoRespostaConfiguracao = await query
                        .AsNoTracking()
                        .ToListAsync();

                    var tiposRespostasDto = TipoRespostaConfiguracaoDto.Mapear(tipoRespostaConfiguracao);

                    return new ListResultDto<TipoRespostaConfiguracaoDto>
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
        public async Task<ListResultDto<TipoRespostaConfiguracaoDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var _tipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracao, long>>())
                {
                    var query = _tipoRespostaConfiguracaoRepositorio.Object
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(filtro) ||
                        m.Descricao.Contains(filtro)
                        );

                    var tipoRespostaConfiguracao = await query
                        .AsNoTracking()
                        .ToListAsync();

                    var tiposRespostasDto = TipoRespostaConfiguracaoDto.Mapear(tipoRespostaConfiguracao);

                    return new ListResultDto<TipoRespostaConfiguracaoDto>
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
            using (var _tipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracao, long>>())
                return await this.CreateSelect2(_tipoRespostaConfiguracaoRepositorio.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [UnitOfWork(false)]
        public Task<FileDto> ListarParaExcel(ListarInput input)
        {
            throw new NotImplementedException();
        }
    }
}
