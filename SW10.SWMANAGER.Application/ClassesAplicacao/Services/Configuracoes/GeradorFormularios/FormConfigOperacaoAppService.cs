namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Threading.Tasks;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;
    using Abp.Collections.Extensions;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Abp.UI;
    using Dapper;
    using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto;

    public class FormConfigOperacaoAppService : SWMANAGERAppServiceBase, IFormConfigOperacaoAppService
    {
        [UnitOfWork]
        public async Task CriarOuEditar(FormConfigOperacaoDto input)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var operacoesIncluidas = string.IsNullOrWhiteSpace(input.OperacoesIncluidas)
                                                 ? new string[0]
                                                 : input.OperacoesIncluidas.Split(',');
                    var operacoesRemovidas = string.IsNullOrWhiteSpace(input.OperacoesRemovidas)
                                                 ? new string[0]
                                                 : input.OperacoesRemovidas.Split(',');
                    foreach (var operacaoId in operacoesIncluidas)
                    {
                        var formConfigOperacao = new FormConfigOperacao
                        {
                            FormConfigId = input.FormConfigId,
                            IsDeleted = false,
                            IsSistema = false,
                            CreatorUserId = this.AbpSession.UserId,
                            OperacaoId = Convert.ToInt64(operacaoId)
                        };
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await formConfigOperacaoRepository.Object.InsertAsync(formConfigOperacao)
                                .ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }

                    foreach (var operacaoId in operacoesRemovidas)
                    {
                        var formConfigOperacao = await this.Obter(input.FormConfigId.Value, Convert.ToInt64(operacaoId))
                                                     .ConfigureAwait(false);

                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await formConfigOperacaoRepository.Object.DeleteAsync(formConfigOperacao.Id)
                                .ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FormConfigOperacaoDto input)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                {
                    await formConfigOperacaoRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<FormConfigOperacaoDto>> Listar(ListarInput input)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                {
                    var query = formConfigOperacaoRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Operacao).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.FormConfig.Nome.Contains(input.Filtro) || m.Operacao.Codigo.Contains(input.Filtro)
                                                                          || m.Operacao.Descricao.Contains(input.Filtro)
                                                                          || m.FormConfig.DataAlteracao
                                                                              .ToShortTimeString()
                                                                              .Contains(input.Filtro));

                    var contarGeradorFormularios = await query.CountAsync().ConfigureAwait(false);

                    var formsConfig = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                          .ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigOperacaoDto>>();

                    return new PagedResultDto<FormConfigOperacaoDto>(contarGeradorFormularios, formsConfigDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigOperacaoDto>> ListarTodos()
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                {
                    var query = formConfigOperacaoRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Operacao);

                    var formsConfig = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigOperacaoDto>>();

                    // var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigOperacaoDto> { Items = formsConfigDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigOperacaoDto>> ListarTodosPorOperacao(long operacaoId)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                {
                    var formsConfig = await formConfigOperacaoRepository.Object.GetAll().AsNoTracking()
                                          .Include(m => m.FormConfig)
                                          .Include(m => m.Operacao)
                                          .Where(m => m.OperacaoId == operacaoId).ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigOperacaoDto>>();

                    // var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigOperacaoDto> { Items = formsConfigDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<IEnumerable<FormConfigDto>> ListarTodosPorOperacaoDapper(long operacaoId)
        {
            try
            {
                var query = @"
                    SELECT 
                        DISTINCT
                        SisFormConfig.* 
                    FROM SisFormConfigOperacao 
                    LEFT JOIN 
                    SisFormConfig 
                        ON SisFormConfigOperacao.SisFormConfigId = SisFormConfig.Id
                    WHERE SisFormConfigOperacao.SisOperacaoId = @operacaoId AND SisFormConfigOperacao.IsDeleted = @deleted";
                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    return await connection.QueryAsync<FormConfigDto>(query, new { operacaoId, deleted = false }).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigOperacaoDto> Obter(long id)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                {
                    var query = formConfigOperacaoRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Operacao).Where(m => m.Id == id);

                    var formConfig = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    var formConfigDto = formConfig.MapTo<FormConfigOperacaoDto>();

                    return formConfigDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                {
                    var query = from p in formConfigOperacaoRepository.Object.GetAll().AsNoTracking()
                                    .Include(m => m.FormConfig).Include(m => m.Operacao).WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(),
                                        m => m.FormConfig.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                                             || m.FormConfig.Nome.ToLower().Contains(dropdownInput.search.ToLower()))
                                orderby p.FormConfig.Nome ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(
                                                   p.FormConfig.Codigo,
                                                   " - ",
                                                   p.FormConfig.Nome,
                                                   " / ",
                                                   p.Operacao.Descricao)
                                };

                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigOperacaoDto>> ListarPorForm(long id)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                {
                    var query = formConfigOperacaoRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Operacao).Where(m => m.FormConfigId == id);

                    var formsConfig = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigOperacaoDto>>();

                    // var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigOperacaoDto> { Items = formsConfigDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigOperacaoDto>> ListarPorOperacao(long id)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                {
                    var query = formConfigOperacaoRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.Operacao).Where(m => m.OperacaoId == id);

                    var formsConfig = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigOperacaoDto>>();

                    // var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigOperacaoDto> { Items = formsConfigDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigOperacaoDto> Obter(long formConfigId, long operacaoId)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                {
                    var query = formConfigOperacaoRepository.Object.GetAll().Include(m => m.FormConfig)
                        .Include(m => m.Operacao)
                        .Where(m => m.FormConfigId == formConfigId && m.OperacaoId == operacaoId);

                    var formConfig = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    var formConfigDto = formConfig.MapTo<FormConfigOperacaoDto>();

                    return formConfigDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<OperacaoDto>> ListarOperacaoPorForm(ListarInput input)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                using (var operacaoService =
                    IocManager.Instance.ResolveAsDisposable<IOperacaoAppService>())
                {
                    long id = 0;
                    var result = long.TryParse(input.PrincipalId, out id);
                    if (!result)
                    {
                        throw new UserFriendlyException(this.L("IdNaoInformado"));
                    }

                    var query = formConfigOperacaoRepository.Object.GetAll().Include(m => m.FormConfig)
                        .Include(m => m.Operacao).Where(m => m.FormConfigId == id).WhereIf(
                            !input.Filtro.IsNullOrWhiteSpace(),
                            m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro))
                        .Select(m => m.Operacao);

                    var query1 = await query.ToListAsync().ConfigureAwait(false);
                    var unidades = query1.Select(m => m.Id).ToArray();

                    // var unidades = associadas.Select(m => m.Id).ToArray();
                    var disponiveis = await operacaoService.Object.ListarTodos().ConfigureAwait(false);
                    var operacoes = disponiveis.Items.Where(m => m.Id.IsIn(unidades) && m.IsFormulario).AsQueryable();

                    var count = operacoes.Count();

                    var operacoesDto = operacoes.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToList();

                    // var operacoesDto = operacoes.MapTo<List<OperacaoDto>>();

                    // var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new PagedResultDto<OperacaoDto> { TotalCount = count, Items = operacoesDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<OperacaoDto>> ListarOperacaoSemForm(ListarInput input)
        {
            try
            {
                using (var formConfigOperacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigOperacao, long>>())
                using (var operacaoService = IocManager.Instance.ResolveAsDisposable<IOperacaoAppService>())
                {
                    long id = 0;
                    var result = long.TryParse(input.PrincipalId, out id);
                    if (!result)
                    {
                        throw new UserFriendlyException(this.L("IdNaoInformado"));
                    }

                    // var associadas = await ListarOperacaoPorForm(input);
                    var query = formConfigOperacaoRepository.Object.GetAll().Include(m => m.FormConfig)
                        .Include(m => m.Operacao).Where(m => m.FormConfigId == id).Select(m => m.Operacao);

                    var associadas = await query.ToListAsync().ConfigureAwait(false);

                    var unidades = associadas.Select(m => m.Id).ToArray();

                    var disponiveis = await operacaoService.Object.ListarTodos().ConfigureAwait(false);

                    var unidadesDisponiveis = disponiveis.Items.Where(m => !m.Id.IsIn(unidades)).ToList();

                    var unidadesFiltradas = unidadesDisponiveis
                        .WhereIf(
                            !input.Filtro.IsNullOrWhiteSpace(),
                            m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro)).WhereIf(
                            input.EmpresaId.HasValue,
                            m => m.ModuloId == input.EmpresaId.Value).Where(m => m.IsFormulario).AsQueryable();

                    var count = unidadesFiltradas.Count();
                    var unidadesDto = unidadesFiltradas.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToList()
                        .MapTo<List<OperacaoDto>>();

                    return new PagedResultDto<OperacaoDto> { TotalCount = count, Items = unidadesDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }
    }
}
