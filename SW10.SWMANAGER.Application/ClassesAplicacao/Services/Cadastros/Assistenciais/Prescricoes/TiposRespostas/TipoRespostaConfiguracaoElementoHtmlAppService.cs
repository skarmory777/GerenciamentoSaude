using Abp.Application.Services.Dto;
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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    public class TipoRespostaConfiguracaoElementoHtmlAppService : SWMANAGERAppServiceBase, ITipoRespostaConfiguracaoElementoHtmlAppService
    {
        [UnitOfWork]
        public async Task<TipoRespostaConfiguracaoElementoHtmlDto> CriarOuEditar(TipoRespostaConfiguracaoElementoHtmlDto input)
        {
            try
            {
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var _tipoRespostaConfiguracaoElementoHtmlRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracaoElementoHtml, long>>())
                {
                    var tipoRespostaConfiguracaoElementoHtml = TipoRespostaConfiguracaoElementoHtmlDto.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _tipoRespostaConfiguracaoElementoHtmlRepositorio.Object.InsertAndGetIdAsync(tipoRespostaConfiguracaoElementoHtml);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            tipoRespostaConfiguracaoElementoHtml = await _tipoRespostaConfiguracaoElementoHtmlRepositorio.Object.UpdateAsync(tipoRespostaConfiguracaoElementoHtml);
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
        public async Task Excluir(TipoRespostaConfiguracaoElementoHtmlDto input)
        {
            try
            {
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var _tipoRespostaConfiguracaoElementoHtmlRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracaoElementoHtml, long>>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    await _tipoRespostaConfiguracaoElementoHtmlRepositorio.Object.DeleteAsync(input.Id);
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
        public async Task<PagedResultDto<TipoRespostaConfiguracaoElementoHtmlDto>> Listar(ListarInput input)
        {
            var contarTipoRespostaConfiguracaoElementoHtml = 0;
            List<TipoRespostaConfiguracaoElementoHtml> tipoRespostaConfiguracaoElementoHtml;
            List<TipoRespostaConfiguracaoElementoHtmlDto> TipoRespostaConfiguracaoElementoHtmlDtos = new List<TipoRespostaConfiguracaoElementoHtmlDto>();
            try
            {
                using (var _tipoRespostaConfiguracaoElementoHtmlRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracaoElementoHtml, long>>())
                {
                    var query = _tipoRespostaConfiguracaoElementoHtmlRepositorio.Object
                    .GetAll()
                    .Include(m => m.TipoRespostaConfiguracao)
                    .Include(m => m.ElementoHtml)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.TipoRespostaConfiguracao.Codigo.Contains(input.Filtro) ||
                        m.TipoRespostaConfiguracao.Descricao.Contains(input.Filtro)
                        );

                    contarTipoRespostaConfiguracaoElementoHtml = await query
                        .CountAsync();

                    tipoRespostaConfiguracaoElementoHtml = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    TipoRespostaConfiguracaoElementoHtmlDtos = TipoRespostaConfiguracaoElementoHtmlDto.Mapear(tipoRespostaConfiguracaoElementoHtml);


                    return new PagedResultDto<TipoRespostaConfiguracaoElementoHtmlDto>(
                        contarTipoRespostaConfiguracaoElementoHtml,
                        TipoRespostaConfiguracaoElementoHtmlDtos
                        );
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoRespostaConfiguracaoElementoHtmlDto>> ListarPorTipoRespostaConfiguracao(ListarTipoRespostaConfiguracaoElementoHtmlInput input)
        {
            var contarTipoRespostaConfiguracaoElementoHtml = 0;
            List<TipoRespostaConfiguracaoElementoHtml> tipoRespostaConfiguracaoElementoHtml;
            List<TipoRespostaConfiguracaoElementoHtmlDto> TipoRespostaConfiguracaoElementoHtmlDtos = new List<TipoRespostaConfiguracaoElementoHtmlDto>();
            try
            {
                using (var _tipoRespostaConfiguracaoElementoHtmlRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracaoElementoHtml, long>>())
                {
                    var query = _tipoRespostaConfiguracaoElementoHtmlRepositorio.Object
                    .GetAll()
                    //.Include(m => m.TipoRespostaConfiguracao)
                    .Include(m => m.ElementoHtml)
                    .Where(m => m.TipoRespostaConfiguracaoId == input.TipoRespostaConfiguracaoId)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.TipoRespostaConfiguracao.Codigo.Contains(input.Filtro) ||
                        m.TipoRespostaConfiguracao.Descricao.Contains(input.Filtro)
                        );

                    contarTipoRespostaConfiguracaoElementoHtml = await query
                        .CountAsync();

                    tipoRespostaConfiguracaoElementoHtml = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    TipoRespostaConfiguracaoElementoHtmlDtos = TipoRespostaConfiguracaoElementoHtmlDto.Mapear(tipoRespostaConfiguracaoElementoHtml);

                    return new PagedResultDto<TipoRespostaConfiguracaoElementoHtmlDto>(
                        contarTipoRespostaConfiguracaoElementoHtml,
                        TipoRespostaConfiguracaoElementoHtmlDtos
                        );
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<TipoRespostaConfiguracaoElementoHtmlDto> Obter(long id)
        {
            try
            {
                using (var _tipoRespostaConfiguracaoElementoHtmlRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracaoElementoHtml, long>>())
                {
                    var result = await _tipoRespostaConfiguracaoElementoHtmlRepositorio.Object
                    .GetAll()
                    .Include(m => m.ElementoHtml)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                    var tipoRespostaConfiguracaoElementoHtml = TipoRespostaConfiguracaoElementoHtmlDto.Mapear(result);

                    return tipoRespostaConfiguracaoElementoHtml;
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoRespostaConfiguracaoElementoHtmlDto>> ListarTodos()
        {
            try
            {
                using (var _tipoRespostaConfiguracaoElementoHtmlRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracaoElementoHtml, long>>())
                {
                    var query = _tipoRespostaConfiguracaoElementoHtmlRepositorio.Object
                    .GetAll();

                    var tipoRespostaConfiguracaoElementoHtml = await query
                        .AsNoTracking()
                        .ToListAsync();

                    var tiposRespostasDto = TipoRespostaConfiguracaoElementoHtmlDto.Mapear(tipoRespostaConfiguracaoElementoHtml);

                    return new ListResultDto<TipoRespostaConfiguracaoElementoHtmlDto>
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
        public async Task<ListResultDto<TipoRespostaConfiguracaoElementoHtmlDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var _tipoRespostaConfiguracaoElementoHtmlRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracaoElementoHtml, long>>())
                {
                    var query = _tipoRespostaConfiguracaoElementoHtmlRepositorio.Object
                    .GetAll()
                    .Include(m => m.TipoRespostaConfiguracao)
                    .Include(m => m.ElementoHtml)
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.TipoRespostaConfiguracao.Codigo.Contains(filtro) ||
                        m.TipoRespostaConfiguracao.Descricao.Contains(filtro)
                        );


                    var tipoRespostaConfiguracaoElementoHtml = await query
                        .AsNoTracking()
                        .ToListAsync();

                    var tiposRespostasDto = TipoRespostaConfiguracaoElementoHtmlDto.Mapear(tipoRespostaConfiguracaoElementoHtml);

                    return new ListResultDto<TipoRespostaConfiguracaoElementoHtmlDto>
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
        public Task<FileDto> ListarParaExcel(ListarInput input)
        {
            throw new NotImplementedException();
        }

        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var _tipoRespostaConfiguracaoElementoHtmlRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaConfiguracaoElementoHtml, long>>())
                {
                    var query = from p in _tipoRespostaConfiguracaoElementoHtmlRepositorio.Object.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.ElementoHtml.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                                          || m.ElementoHtml.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                                                      )
                                orderby p.ElementoHtml.Descricao ascending
                                select new DropdownItems { id = p.Id, text = string.Concat(p.ElementoHtml.Codigo.ToString(), " - ", p.ElementoHtml.Descricao) };

                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync();

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
