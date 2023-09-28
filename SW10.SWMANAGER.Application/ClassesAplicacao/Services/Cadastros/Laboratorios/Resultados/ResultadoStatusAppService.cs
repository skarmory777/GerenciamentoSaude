using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados
{
    public class ResultadoStatusAppService : SWMANAGERAppServiceBase, IResultadoStatusAppService
    {
        [UnitOfWork]
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(ResultadoStatusDto input)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    using (var resultadoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoStatus, long>>())
                    using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                    {
                        var resultadoStatus = ResultadoStatusDto.Mapear(input);

                        if (input.Id.Equals(0))
                        {
                            resultadoStatus.Codigo = await ultimoIdAppService.Object.ObterProximoCodigo("ResultadoStatus").ConfigureAwait(false);
                            input.Id = await resultadoStatusRepositorio.Object.InsertAndGetIdAsync(resultadoStatus).ConfigureAwait(false);
                        }
                        else
                        {
                            var result = await resultadoStatusRepositorio.Object.GetAsync(input.Id).ConfigureAwait(false);
                            result.Codigo = input.Codigo;
                            result.CorFonte = input.CorFonte;
                            result.CorFundo = input.CorFundo;
                            result.Descricao = input.Descricao;
                            result.IsAtivo = input.IsAtivo;
                            result.IsSistema = input.IsSistema;
                            result.Sequencia = input.Sequencia;

                            await resultadoStatusRepositorio.Object.UpdateAsync(result).ConfigureAwait(false);
                        }

                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        unitOfWorkManager.Object.Current.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(ResultadoStatusDto input)
        {
            try
            {
                using (var resultadoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoStatus, long>>())
                {
                    await resultadoStatusRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ResultadoStatusDto>> Listar(ListarInput input)
        {
            var contar = 0;
            List<ResultadoStatus> resultados;
            List<ResultadoStatusDto> resultadosDtos = new List<ResultadoStatusDto>();
            try
            {
                using (var resultadoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoStatus, long>>())
                {
                    var query = resultadoStatusRepositorio.Object.GetAll().AsNoTracking().WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Codigo.Contains(input.Filtro) || m.Descricao.ToUpper().Contains(input.Filtro.ToUpper()));

                    contar = await query.CountAsync().ConfigureAwait(false);

                    resultados = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                     .ConfigureAwait(false);

                    resultadosDtos = ResultadoStatusDto.Mapear(resultados).ToList();
                    var result = resultadosDtos.AsQueryable().AsNoTracking().OrderBy(input.Sorting).PageBy(input)
                        .ToList();

                    return new PagedResultDto<ResultadoStatusDto>(contar, result);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultadoStatusDto> Obter(long id)
        {
            try
            {
                using (var resultadoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoStatus, long>>())
                {
                    var query = resultadoStatusRepositorio.Object.GetAll().AsNoTracking().Where(m => m.Id == id);

                    var resultado = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    var resultadoDto = ResultadoStatusDto.Mapear(resultado); //.MapTo<ResultadoDto>();

                    return resultadoDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<ResultadoStatusDto> resultadosStatusDtos = new List<ResultadoStatusDto>();
            try
            {
                using (var resultadoStatusRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoStatus, long>>())
                {

                    //get com filtro
                    var query = from p in resultadoStatusRepositorio.Object.GetAll().AsNoTracking().WhereIf(
                                    !dropdownInput.search.IsNullOrEmpty(),
                                    m => m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                    //||
                                    //  m.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                                )
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };
                    //paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync().ConfigureAwait(false);

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
