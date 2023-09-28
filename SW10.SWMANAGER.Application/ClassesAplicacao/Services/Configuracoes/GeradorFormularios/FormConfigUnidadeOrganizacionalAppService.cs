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
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;

    public class FormConfigUnidadeOrganizacionalAppService : SWMANAGERAppServiceBase, IFormConfigUnidadeOrganizacionalAppService
    {

        [UnitOfWork]
        public async Task CriarOuEditar(FormConfigUnidadeOrganizacionalDto input)
        {
            try
            {
                var unidadesIncluidas = string.IsNullOrWhiteSpace(input.UnidadesIncluidas)
                                            ? new string[0]
                                            : input.UnidadesIncluidas.Split(',');
                var unidadesRemovidas = string.IsNullOrWhiteSpace(input.UnidadesRemovidas)
                                            ? new string[0]
                                            : input.UnidadesRemovidas.Split(',');

                using (var formConfigUnidadeOrganizacionalRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    foreach (var unidadeOrganizacionalId in unidadesIncluidas)
                    {
                        var formConfigUnidadeOrganizacional = new FormConfigUnidadeOrganizacional
                        {
                            FormConfigId = input.FormConfigId,
                            IsDeleted = false,
                            IsSistema = false,
                            CreatorUserId = this.AbpSession.UserId,
                            UnidadeOrganizacionalId =
                                                                          Convert.ToInt64(unidadeOrganizacionalId)
                        };
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await formConfigUnidadeOrganizacionalRepository.Object
                                .InsertAsync(formConfigUnidadeOrganizacional).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }

                    foreach (var unidadeOrganizacionalId in unidadesRemovidas)
                    {
                        var formConfigUnidadeOrganizacional = await this.Obter(
                                                                      input.FormConfigId.Value,
                                                                      Convert.ToInt64(unidadeOrganizacionalId))
                                                                  .ConfigureAwait(false);

                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await formConfigUnidadeOrganizacionalRepository.Object.DeleteAsync(formConfigUnidadeOrganizacional.Id).ConfigureAwait(false);
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

        public async Task Excluir(FormConfigUnidadeOrganizacionalDto input)
        {
            try
            {
                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                {
                    await formConfigUnidadeOrganizacionalRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<FormConfigUnidadeOrganizacionalDto>> Listar(ListarInput input)
        {
            try
            {
                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                {
                    var query = formConfigUnidadeOrganizacionalRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.UnidadeOrganizacional).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.FormConfig.Nome.Contains(input.Filtro)
                                 || m.UnidadeOrganizacional.Codigo.Contains(input.Filtro)
                                 || m.UnidadeOrganizacional.Descricao.Contains(input.Filtro)
                                 || m.FormConfig.DataAlteracao.ToShortTimeString().Contains(input.Filtro));

                    var contarGeradorFormularios = await query.CountAsync().ConfigureAwait(false);

                    var formsConfig = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                          .ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigUnidadeOrganizacionalDto>>();

                    return new PagedResultDto<FormConfigUnidadeOrganizacionalDto>(
                        contarGeradorFormularios,
                        formsConfigDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigUnidadeOrganizacionalDto>> ListarTodos()
        {
            try
            {
                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                {
                    var query = formConfigUnidadeOrganizacionalRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.UnidadeOrganizacional);


                    var formsConfig = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigUnidadeOrganizacionalDto>>();

                    // var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigUnidadeOrganizacionalDto> { Items = formsConfigDto };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigUnidadeOrganizacionalDto>> ListarTodosPorUnidade(long unidadeOrganizacional)
        {
            try
            {
                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                {
                    var formsConfig = await formConfigUnidadeOrganizacionalRepository.Object.GetAll().AsNoTracking()
                                          .Include(m => m.FormConfig).Include(m => m.UnidadeOrganizacional)
                                          .Where(c => c.UnidadeOrganizacionalId == unidadeOrganizacional).AsNoTracking()
                                          .ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigUnidadeOrganizacionalDto>>();
                    return new ListResultDto<FormConfigUnidadeOrganizacionalDto> { Items = formsConfigDto };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }


        public async Task<IEnumerable<FormConfigDto>> ListarTodosPorUnidadeDapper(long unidadeOrganizacional)
        {
            try
            {
                var query = @"SELECT 
                    DISTINCT SisFormConfig.*
                    FROM SisFormConfigUnidadeOrganizacional 
                    INNER JOIN SisFormConfig ON SisFormConfig.Id = SisFormConfigUnidadeOrganizacional.SisFormConfigId 
                    INNER JOIN SisUnidadeOrganizacional ON SisFormConfigUnidadeOrganizacional.SisUnidadeOganizacionalId = SisUnidadeOrganizacional.Id
                    WHERE SisFormConfigUnidadeOrganizacional.SisUnidadeOganizacionalId = @unidadeOrganizacional AND SisFormConfigUnidadeOrganizacional.IsDeleted = @deleted";
                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    return await connection.QueryAsync<FormConfigDto>(query, new { unidadeOrganizacional,deleted = false }).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigUnidadeOrganizacionalDto> Obter(long id)
        {
            try
            {
                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                {
                    var query = formConfigUnidadeOrganizacionalRepository.Object.GetAll().Include(m => m.FormConfig)
                        .Include(m => m.UnidadeOrganizacional).Where(m => m.Id == id);

                    var formConfig = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    var formConfigDto = formConfig.MapTo<FormConfigUnidadeOrganizacionalDto>();

                    return formConfigDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormConfigUnidadeOrganizacionalDto> Obter(long formConfigId, long unidadeOrganizacionalId)
        {
            try
            {
                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                {
                    var query = formConfigUnidadeOrganizacionalRepository.Object.GetAll().Include(m => m.FormConfig)
                        .Include(m => m.UnidadeOrganizacional).Where(
                            m => m.FormConfigId == formConfigId
                                 && m.UnidadeOrganizacionalId == unidadeOrganizacionalId);

                    var formConfig = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                    var formConfigDto = formConfig.MapTo<FormConfigUnidadeOrganizacionalDto>();

                    return formConfigDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigUnidadeOrganizacionalDto>> ListarPorForm(long id)
        {
            try
            {
                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                {
                    var query = formConfigUnidadeOrganizacionalRepository.Object.GetAll().Include(m => m.FormConfig)
                        .Include(m => m.UnidadeOrganizacional).Where(m => m.FormConfigId == id);


                    var formsConfig = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigUnidadeOrganizacionalDto>>();

                    // var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigUnidadeOrganizacionalDto> { Items = formsConfigDto };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<UnidadeOrganizacionalDto>> ListarUnidadeOrganizacionalPorForm(ListarInput input)
        {
            try
            {
                long id = 0;
                var result = long.TryParse(input.PrincipalId, out id);
                if (!result)
                {
                    throw new UserFriendlyException(this.L("IdNaoInformado"));
                }

                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                using (var unidadeOrganizacionalService = IocManager.Instance
                    .ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                {
                    var query = formConfigUnidadeOrganizacionalRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.UnidadeOrganizacional.OrganizationUnit)
                        .Where(m => m.FormConfigId == id).WhereIf(
                            !input.Filtro.IsNullOrWhiteSpace(),
                            m => m.UnidadeOrganizacional.OrganizationUnit.Code.Contains(input.Filtro)
                                 || m.UnidadeOrganizacional.OrganizationUnit.DisplayName.Contains(input.Filtro)
                                 || m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro))
                        .Select(m => m.UnidadeOrganizacional);

                    var query1 = await query.ToListAsync().ConfigureAwait(false);
                    var unidades = query1.Select(m => m.Id).ToArray();

                    // var unidades = associadas.Select(m => m.Id).ToArray();
                    var disponiveis = await unidadeOrganizacionalService.Object.ListarTodos().ConfigureAwait(false);
                    var unidadesOrganizacionais = disponiveis.Items
                        .Where(m => m.Id.IsIn(unidades) && (m.IsAmbulatorioEmergencia || m.IsInternacao || m.IsControlaLeito)).AsQueryable();

                    var count = unidadesOrganizacionais.Count();

                    var unidadesOrganizacionaisDto = unidadesOrganizacionais.AsNoTracking().OrderBy(input.Sorting)
                        .PageBy(input).ToList();


                    // var unidadesOrganizacionaisDto = unidadesOrganizacionais.MapTo<List<UnidadeOrganizacionalDto>>();

                    // var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new PagedResultDto<UnidadeOrganizacionalDto>
                    {
                        TotalCount = count,
                        Items = unidadesOrganizacionaisDto
                    };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<UnidadeOrganizacionalDto>> ListarUnidadeOrganizacionalSemForm(ListarInput input)
        {
            try
            {
                long id = 0;
                var result = long.TryParse(input.PrincipalId, out id);
                if (!result)
                {
                    throw new UserFriendlyException(this.L("IdNaoInformado"));
                }

                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                using (var unidadeOrganizacionalService = IocManager.Instance
                    .ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                {
                    // var associadas = await ListarUnidadeOrganizacionalPorForm(input);
                    var query = formConfigUnidadeOrganizacionalRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.UnidadeOrganizacional.OrganizationUnit)
                        .Where(m => m.FormConfigId == id).Select(m => m.UnidadeOrganizacional);

                    var associadas = await query.ToListAsync().ConfigureAwait(false);

                    var unidades = associadas.Select(m => m.Id).ToArray();

                    var disponiveis = await unidadeOrganizacionalService.Object.ListarTodos().ConfigureAwait(false);

                    var unidadesDisponiveis = disponiveis.Items
                        .Where(m => !m.Id.IsIn(unidades) && (m.IsAmbulatorioEmergencia || m.IsInternacao || m.IsControlaLeito)).ToList();

                    var unidadesFiltradas = unidadesDisponiveis.WhereIf(
                        !input.Filtro.IsNullOrWhiteSpace(),
                        m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro)).AsQueryable();

                    var count = unidadesFiltradas.Count();
                    var unidadesDto = unidadesFiltradas.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToList()
                        .MapTo<List<UnidadeOrganizacionalDto>>();

                    return new PagedResultDto<UnidadeOrganizacionalDto> { TotalCount = count, Items = unidadesDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FormConfigUnidadeOrganizacionalDto>> ListarPorUnidadeOrganizacional(long id)
        {
            try
            {
                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                {
                    var query = formConfigUnidadeOrganizacionalRepository.Object.GetAll().AsNoTracking().Include(m => m.FormConfig)
                        .Include(m => m.UnidadeOrganizacional).Where(m => m.UnidadeOrganizacionalId == id);


                    var formsConfig = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var formsConfigDto = formsConfig.MapTo<List<FormConfigUnidadeOrganizacionalDto>>();


                    // var formsConfig = query.MapTo<List<FormConfigDto>>();
                    return new ListResultDto<FormConfigUnidadeOrganizacionalDto> { Items = formsConfigDto };
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
                using (var formConfigUnidadeOrganizacionalRepository = IocManager.Instance
                    .ResolveAsDisposable<IRepository<FormConfigUnidadeOrganizacional, long>>())
                {
                    var query = from p in formConfigUnidadeOrganizacionalRepository.Object.GetAll().AsNoTracking()
                                    .Include(m => m.FormConfig).Include(m => m.UnidadeOrganizacional).WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(),
                                        m => m.FormConfig.Codigo.Contains(dropdownInput.search)
                                             || m.FormConfig.Nome.Contains(dropdownInput.search))
                                orderby p.FormConfig.Nome ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(
                                                   p.FormConfig.Codigo,
                                                   " - ",
                                                   p.FormConfig.Nome,
                                                   " / ",
                                                   p.UnidadeOrganizacional.Descricao)
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
    }
}
