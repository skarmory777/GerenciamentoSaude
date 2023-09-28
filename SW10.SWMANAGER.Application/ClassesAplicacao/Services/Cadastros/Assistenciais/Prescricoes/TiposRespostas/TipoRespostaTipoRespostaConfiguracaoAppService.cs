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
    public class TipoRespostaTipoRespostaConfiguracaoAppService : SWMANAGERAppServiceBase, ITipoRespostaTipoRespostaConfiguracaoAppService
    {
        [UnitOfWork]
        public async Task<TipoRespostaTipoRespostaConfiguracaoDto> CriarOuEditar(TipoRespostaTipoRespostaConfiguracaoDto input)
        {
            try
            {
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var _tipoRespostaTipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaTipoRespostaConfiguracao, long>>())
                {
                    var tipoRespostaTipoRespostaConfiguracao = TipoRespostaTipoRespostaConfiguracaoDto.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await _tipoRespostaTipoRespostaConfiguracaoRepositorio.Object.InsertAndGetIdAsync(tipoRespostaTipoRespostaConfiguracao);
                            unitOfWork.Complete();
                            _unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                        {
                            tipoRespostaTipoRespostaConfiguracao = await _tipoRespostaTipoRespostaConfiguracaoRepositorio.Object.UpdateAsync(tipoRespostaTipoRespostaConfiguracao);
                            unitOfWork.Complete();
                            _unitOfWorkManager.Object.Current.SaveChanges();
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
        public async Task Excluir(TipoRespostaTipoRespostaConfiguracaoDto input)
        {
            try
            {
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var _tipoRespostaTipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaTipoRespostaConfiguracao, long>>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    await _tipoRespostaTipoRespostaConfiguracaoRepositorio.Object.DeleteAsync(input.Id);
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
        public async Task<PagedResultDto<TipoRespostaTipoRespostaConfiguracaoDto>> Listar(ListarInput input)
        {
            var contarTipoRespostaTipoRespostaConfiguracao = 0;
            List<TipoRespostaTipoRespostaConfiguracao> tipoRespostaTipoRespostaConfiguracao;
            List<TipoRespostaTipoRespostaConfiguracaoDto> TipoRespostaTipoRespostaConfiguracaoDtos = new List<TipoRespostaTipoRespostaConfiguracaoDto>();
            try
            {
                using (var _tipoRespostaTipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaTipoRespostaConfiguracao, long>>())
                {
                    var query = _tipoRespostaTipoRespostaConfiguracaoRepositorio.Object
                    .GetAll()
                    .Include(m => m.TipoResposta)
                    .Include(m => m.TipoRespostaConfiguracao)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.TipoResposta.Codigo.Contains(input.Filtro) ||
                        m.TipoResposta.Descricao.Contains(input.Filtro)
                        );

                    contarTipoRespostaTipoRespostaConfiguracao = await query
                        .CountAsync();

                    tipoRespostaTipoRespostaConfiguracao = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    TipoRespostaTipoRespostaConfiguracaoDtos = TipoRespostaTipoRespostaConfiguracaoDto.Mapear(tipoRespostaTipoRespostaConfiguracao);


                    return new PagedResultDto<TipoRespostaTipoRespostaConfiguracaoDto>(
                        contarTipoRespostaTipoRespostaConfiguracao,
                        TipoRespostaTipoRespostaConfiguracaoDtos
                        );
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoRespostaTipoRespostaConfiguracaoDto>> ListarPorTipoResposta(ListarTipoRespostaTipoRespostaConfiguracaoInput input)
        {
            var contarTipoRespostaConfiguracaoElementoHtml = 0;
            List<TipoRespostaTipoRespostaConfiguracao> tipoRespostaConfiguracaoElementoHtml;
            List<TipoRespostaTipoRespostaConfiguracaoDto> TipoRespostaConfiguracaoElementoHtmlDtos = new List<TipoRespostaTipoRespostaConfiguracaoDto>();
            try
            {
                using (var _tipoRespostaTipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaTipoRespostaConfiguracao, long>>())
                {
                    var query = _tipoRespostaTipoRespostaConfiguracaoRepositorio.Object
                    .GetAll()
                    //.Include(m => m.TipoRespostaConfiguracao)
                    .Include(m => m.TipoRespostaConfiguracao)
                    .Where(m => m.TipoRespostaId == input.TipoRespostaId)
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

                    TipoRespostaConfiguracaoElementoHtmlDtos = TipoRespostaTipoRespostaConfiguracaoDto.Mapear(tipoRespostaConfiguracaoElementoHtml);

                    return new PagedResultDto<TipoRespostaTipoRespostaConfiguracaoDto>(
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
        public async Task<TipoRespostaTipoRespostaConfiguracaoDto> Obter(long id)
        {
            try
            {
                using (var _tipoRespostaTipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaTipoRespostaConfiguracao, long>>())
                {
                    var result = await _tipoRespostaTipoRespostaConfiguracaoRepositorio.Object
                    .GetAll()
                    .Where(m => m.Id == id)
                    .Include(m => m.TipoRespostaConfiguracao)
                    .FirstOrDefaultAsync();

                    var tipoRespostaTipoRespostaConfiguracao = TipoRespostaTipoRespostaConfiguracaoDto.Mapear(result);

                    return tipoRespostaTipoRespostaConfiguracao;
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<TipoRespostaTipoRespostaConfiguracaoDto>> ListarTodos()
        {
            try
            {
                using (var _tipoRespostaTipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaTipoRespostaConfiguracao, long>>())
                {
                    var query = _tipoRespostaTipoRespostaConfiguracaoRepositorio.Object
                    .GetAll();

                    var tipoRespostaTipoRespostaConfiguracao = await query
                        .AsNoTracking()
                        .ToListAsync();

                    var tiposRespostasDto = TipoRespostaTipoRespostaConfiguracaoDto.Mapear(tipoRespostaTipoRespostaConfiguracao);

                    return new ListResultDto<TipoRespostaTipoRespostaConfiguracaoDto>
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
        public async Task<ListResultDto<TipoRespostaTipoRespostaConfiguracaoDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var _tipoRespostaTipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaTipoRespostaConfiguracao, long>>())
                {
                    var query = _tipoRespostaTipoRespostaConfiguracaoRepositorio.Object
                    .GetAll()
                    .Include(m => m.TipoResposta)
                    .Include(m => m.TipoRespostaConfiguracao)
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.TipoResposta.Codigo.Contains(filtro) ||
                        m.TipoResposta.Descricao.Contains(filtro)
                        );

                    var tipoRespostaTipoRespostaConfiguracao = await query
                        .AsNoTracking()
                        .ToListAsync();

                    var tiposRespostasDto = TipoRespostaTipoRespostaConfiguracaoDto.Mapear(tipoRespostaTipoRespostaConfiguracao);

                    return new ListResultDto<TipoRespostaTipoRespostaConfiguracaoDto>
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
                using (var _tipoRespostaTipoRespostaConfiguracaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<TipoRespostaTipoRespostaConfiguracao, long>>())
                {
                    var query = from p in _tipoRespostaTipoRespostaConfiguracaoRepositorio.Object.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.TipoRespostaConfiguracao.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                                          || m.TipoRespostaConfiguracao.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                                                      )
                                orderby p.TipoRespostaConfiguracao.Descricao ascending
                                select new DropdownItems { id = p.Id, text = string.Concat(p.TipoRespostaConfiguracao.Codigo.ToString(), " - ", p.TipoRespostaConfiguracao.Descricao) };

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
