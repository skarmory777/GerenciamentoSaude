namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais
{
    using Abp.Application.Services.Dto;
    using Abp.Auditing;
    using Abp.Authorization;
    using Abp.Authorization.Users;
    using Abp.AutoMapper;
    using Abp.Collections.Extensions;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Abp.UI;
    using Dapper;
    using SW10.SWMANAGER.Authorization;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Exporting;
    using SW10.SWMANAGER.Dto;
    using SW10.SWMANAGER.Helpers;
    using SW10.SWMANAGER.Organizations;
    using SW10.SWMANAGER.Organizations.Dto;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Text;
    using System.Threading.Tasks;

    public class UnidadeOrganizacionalAppService : SWMANAGERAppServiceBase, IUnidadeOrganizacionalAppService
    {
        #region Dependencias


        private readonly IIocManager _iocManager;

        public UnidadeOrganizacionalAppService(IIocManager iocManager)
        {
            _iocManager = iocManager;
        }

        #endregion dependencias.

        public async Task<PagedResultDto<UnidadeOrganizacionalDto>> Listar(ListarUnidadesOrganizacionaisInput input)
        {
            try
            {
                using (var unidadeOrganizacionalRepositorio = this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var query = unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking()
                        .Include(m => m.OrganizationUnit).Include(m => m.UnidadeInternacaoTipo).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.Descricao.Contains(input.Filtro));

                    var contarUnidadesOrganizacionais = await query.CountAsync().ConfigureAwait(false);

                    var unidades = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    var unidadesOrganizacionaisDtos = unidades.Select(UnidadeOrganizacionalDto.Mapear).ToList();
                    return new PagedResultDto<UnidadeOrganizacionalDto>(contarUnidadesOrganizacionais, unidadesOrganizacionaisDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<UnidadeOrganizacionalDto>> ListarTodos()
        {
            try
            {
                using (var unidadeOrganizacionalRepositorio = this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                using (var organizationUnitAppService = this._iocManager.ResolveAsDisposable<IOrganizationUnitAppService>())
                {
                    var unidadesOrganizacionais = await unidadeOrganizacionalRepositorio.Object.GetAll()
                                                      .AsNoTracking()
                                                      .Include(m => m.OrganizationUnit)
                                                      .Include(m => m.UnidadeInternacaoTipo).AsNoTracking()
                                                      .ToListAsync().ConfigureAwait(false);
                    var ous = await organizationUnitAppService.Object.GetOrganizationUnits().ConfigureAwait(false);
                    foreach (var unidade in unidadesOrganizacionais)
                    {
                        var ou = ous.Items.FirstOrDefault(o => o.Id == unidade.OrganizationUnitId);

                        if (ou != null)
                        {
                            unidade.OrganizationUnit = OrganizationUnitDto.Mapear(ou);
                        }
                    }

                    var unidadesOrganizacionaisDtos = unidadesOrganizacionais.Select(UnidadeOrganizacionalDto.Mapear).ToList();

                    return new ListResultDto<UnidadeOrganizacionalDto> { Items = unidadesOrganizacionaisDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

       

        public async Task<ListResultDto<UnidadeOrganizacionalDto>> ListarParaAmbulatorioEmergencia()
        {

            try
            {
                using (var unidadeOrganizacionalRepositorio = this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var unidadesOrganizacionais = await unidadeOrganizacionalRepositorio.Object.GetAll()
                                                  .Include(m => m.OrganizationUnit)
                                                  .Where(u => u.ControlaAlta && u.IsAmbulatorioEmergencia)
                                                  .AsNoTracking().ToListAsync().ConfigureAwait(false);
                    return new ListResultDto<UnidadeOrganizacionalDto> { Items = unidadesOrganizacionais.Select(UnidadeOrganizacionalDto.Mapear).ToList() };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<UnidadeOrganizacionalDto>> ListarParaInternacao()
        {
            List<UnidadeOrganizacional> unidadesOrganizacionais;
            var unidadesOrganizacionaisDtos = new List<UnidadeOrganizacionalDto>();
            try
            {
                using (var unidadeOrganizacionalRepositorio = this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                using (var userOrganizationUnitRepository =
                    this._iocManager.ResolveAsDisposable<IRepository<UserOrganizationUnit, long>>())
                {
                    var listaUnidades = from userOrg in userOrganizationUnitRepository.Object.GetAll().AsNoTracking()
                                        join org in unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking() on
                                            userOrg.OrganizationUnitId equals org.Id
                                        where userOrg.UserId == this.AbpSession.UserId
                                        select org;

                    // .Where(w => w.UserId == AbpSession.UserId);
                    unidadesOrganizacionais = await unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking()
                                                  .Where(
                                                      w => listaUnidades.Any(
                                                          a => w.OrganizationUnit.Code.StartsWith(
                                                              a.OrganizationUnit.Code)))
                                                  .Include(m => m.OrganizationUnit)

                                                  // .Include(m => m.UnidadeInternacaoTipo)
                                                  .Where(u => u.ControlaAlta && u.IsControlaLeito).ToListAsync()
                                                  .ConfigureAwait(false);
                    unidadesOrganizacionaisDtos =
                        unidadesOrganizacionais.Select(UnidadeOrganizacionalDto.Mapear).ToList();

                    return new ListResultDto<UnidadeOrganizacionalDto> { Items = unidadesOrganizacionaisDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        // [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_UnidadeOrganizacional_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_UnidadeOrganizacional_Edit)]
        public async Task CriarOuEditar(UnidadeOrganizacionalDto input)
        {
            try
            {
                using (var _unidadeOrganizacionalRepositorio =
                    this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var unidadeOrganizacional = input.MapTo<UnidadeOrganizacional>();
                    if (input.Id.Equals(0))
                    {
                        await _unidadeOrganizacionalRepositorio.Object.InsertOrUpdateAsync(unidadeOrganizacional)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        await _unidadeOrganizacionalRepositorio.Object.UpdateAsync(unidadeOrganizacional)
                            .ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroSalvar"), ex);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_UnidadeOrganizacional_Delete)]
        public async Task Excluir(long id)
        {
            try
            {
                using (var unidadeOrganizacionalRepositorio = this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var unidadeOrganizacional = unidadeOrganizacionalRepositorio.Object.GetAll()
                        .FirstOrDefault(u => u.OrganizationUnitId == id);

                    if (unidadeOrganizacional == null)
                    {
                        return;
                    }

                    var unidadeOrganizacionalId = UnidadeOrganizacionalDto.Mapear(unidadeOrganizacional)?.Id ?? 0;

                    await unidadeOrganizacionalRepositorio.Object.DeleteAsync(unidadeOrganizacionalId).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }
        }

        public async Task<UnidadeOrganizacionalDto> Obter(long id)
        {
            try
            {
                using (var unidadeOrganizacionalRepositorio =
                    this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var result = await unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking().Include(m => m.OrganizationUnit)
                                     .Include(m => m.UnidadeInternacaoTipo).Include(m => m.Estoque).AsNoTracking()
                                     .FirstOrDefaultAsync(u => u.OrganizationUnitId == id).ConfigureAwait(false);
                    var unidadeOrganizacional = UnidadeOrganizacionalDto.MapearFromCore(result);
                    return unidadeOrganizacional;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<UnidadeOrganizacionalDto> ObterPorId(long id)
        {
            try
            {
                using (var unidadeOrganizacionalRepositorio =
                    this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var result = await unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking().Where(w => w.Id == id)
                                     .Include(m => m.OrganizationUnit).Include(m => m.UnidadeInternacaoTipo)
                                     .FirstOrDefaultAsync().ConfigureAwait(false);
                    var unidadeOrganizacional = result.MapTo<UnidadeOrganizacionalDto>();
                    return unidadeOrganizacional;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public UnidadeOrganizacionalDto ObterPorIdSync(long id)
        {
            try
            {
                using (var unidadeOrganizacionalRepositorio =
                    this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var result = unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking().Where(w => w.Id == id)
                                     .Include(m => m.OrganizationUnit).Include(m => m.UnidadeInternacaoTipo)
                                     .FirstOrDefault();
                    var unidadeOrganizacional = result.MapTo<UnidadeOrganizacionalDto>();
                    return unidadeOrganizacional;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<IEnumerable<UnidadeOrganizacionalDto>> ObterIds(List<long> ids)
        {
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    return null;
                }

                var query = $@"
                    SELECT 
                        {QueryHelper.CreateQueryFields<UnidadeOrganizacional>(tableAlias: "UnidadeOrganizacional").IgnoreField(x => x.OrganizationUnit).GetFields()},
                        {QueryHelper.CreateQueryFields<Abp.Organizations.OrganizationUnit>(tableAlias: "OrganizationUnit").IgnoreField(x => x.Parent).GetFields()},
                        {QueryHelper.CreateQueryFields<UnidadeInternacaoTipo>(tableAlias: "UnidadeInternacaoTipo").GetFields()}
                    FROM 
                        SisUnidadeOrganizacional AS UnidadeOrganizacional
                        LEFT JOIN AbpOrganizationUnits as OrganizationUnit ON UnidadeOrganizacional.SisOrganizationUnitId = OrganizationUnit.Id
                        LEFT JOIN AteUnidadeInternacaoTipo as UnidadeInternacaoTipo ON UnidadeOrganizacional.AteUnidadeInternacaoTipoId = UnidadeInternacaoTipo.Id
                    WHERE 
                        UnidadeOrganizacional.Id IN @ids
                        AND UnidadeOrganizacional.IsDeleted = 0
                    ";

                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    return await sqlConnection.QueryAsync(
                        query, 
                        (Func<UnidadeOrganizacionalDto, OrganizationUnitDto, UnidadeInternacaoTipoDto, UnidadeOrganizacionalDto>)DapperMapper, 
                        new { ids = ids.Distinct().ToList() });
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        private static UnidadeOrganizacionalDto DapperMapper(UnidadeOrganizacionalDto unidadeOrganizacional, OrganizationUnitDto organizationUnit, UnidadeInternacaoTipoDto unidadeInternacaoTipo)
        {
            if (unidadeOrganizacional == null)
            {
                return null;
            }

            if (organizationUnit != null)
            {
                unidadeOrganizacional.OrganizationUnit = organizationUnit;
            }

            if (organizationUnit != null)
            {
                unidadeOrganizacional.UnidadeInternacaoTipo = unidadeInternacaoTipo;
            }

            return unidadeOrganizacional;

        }

        public async Task<FileDto> ListarParaExcel(ListarUnidadesOrganizacionaisInput input)
        {
            try
            {
                using (var listarUnidadesOrganizacionaisExcelExporter =
                    this._iocManager.ResolveAsDisposable<IListarUnidadesOrganizacionaisExcelExporter>())
                {
                    var query = await this.Listar(input).ConfigureAwait(false);

                    var unidadesOrganizacionaisDtos = query.Items;

                    return listarUnidadesOrganizacionaisExcelExporter.Object.ExportToFile(
                        unidadesOrganizacionaisDtos.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(this.L("ErroExportar"));
            }
        }

        public async Task<string> ChecarControlaAlta(long id)
        {
            var uo = await this.Obter(id).ConfigureAwait(false);

            if (uo.ControlaAlta)
            {
                return true.ToString().ToLower();
            }
            else
            {
                return false.ToString().ToLower();
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var _unidadeOrganizacionalRepositorio =
                this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
            {
                return await this.CreateSelect2(_unidadeOrganizacionalRepositorio.Object).AddWhereMethod(
                           (input, dapperParameters) =>
                               {
                                   var whereBuilder = new StringBuilder(
                                       Select2Helper.DefaultWhereMethod(input, dapperParameters));

                                   whereBuilder.WhereIf(
                                       dropdownInput.filtro == "inter",
                                       " AND IsInternacao = @IsInternacao");

                                   whereBuilder.WhereIf(
                                       dropdownInput.filtro == "ambEmr",
                                       " AND IsAmbulatorioEmergencia = @IsAmbulatorioEmergencia");

                                   dapperParameters.Add("IsInternacao", dropdownInput.filtro == "inter");
                                   dapperParameters.Add("IsAmbulatorioEmergencia", dropdownInput.filtro == "ambEmr");

                                   return whereBuilder.ToString();
                               }).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownEstoque(DropdownInput dropdownInput)
        {
            using (var _unidadeOrganizacionalRepositorio =
                this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
            {
                return await this.CreateSelect2(_unidadeOrganizacionalRepositorio.Object).AddWhereMethod(
                           (input, dapperParameters) =>
                           {
                               var whereBuilder = new StringBuilder(
                                   Select2Helper.DefaultWhereMethod(input, dapperParameters));

                               whereBuilder.Append(" AND IsEstoque = @IsEstoque");

                               dapperParameters.Add("IsEstoque", true);

                               return whereBuilder.ToString();
                           }).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownPorUsuario(DropdownInput dropdownInput)
        {
            using (var unidadeOrganizacionalRepositorio = this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
            using (var userOrganizationUnitRepository = this._iocManager.ResolveAsDisposable<IRepository<UserOrganizationUnit, long>>())
            {
                var organizationUnitIds = userOrganizationUnitRepository.Object.GetAll().AsNoTracking()
                    .Where(w => w.UserId == this.AbpSession.UserId).Select(s => s.OrganizationUnitId).ToList();

                return await this.CreateSelect2(unidadeOrganizacionalRepositorio.Object).AddWhereMethod(
                           (input, dapperParameters) =>
                               {
                                   var whereBuilder = new StringBuilder(
                                       Select2Helper.DefaultWhereMethod(input, dapperParameters));

                                   whereBuilder.WhereIf(
                                       organizationUnitIds.Any(),
                                       " AND SisOrganizationUnitId IN @organizationUnitIds ");

                                   whereBuilder.WhereIf(
                                       dropdownInput.filtro == "inter",
                                       " AND IsInternacao = @IsInternacao");

                                   whereBuilder.WhereIf(
                                       dropdownInput.filtro == "ambEmr",
                                       " AND IsAmbulatorioEmergencia = @IsAmbulatorioEmergencia");

                                   dapperParameters.Add("IsInternacao", dropdownInput.filtro == "inter");
                                   dapperParameters.Add("IsAmbulatorioEmergencia", dropdownInput.filtro == "ambEmr");

                                   dapperParameters.Add("organizationUnitIds", organizationUnitIds);

                                   return whereBuilder.ToString();
                               }).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownUnidadeAtual(DropdownInput dropdownInput)
        {

            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            var unidadeOrganizacionaisDtos = new List<UnidadeOrganizacionalDto>();
            try
            {
                using (var _unidadeOrganizacionalRepositorio = this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                using (var _leitoRepositorio = this._iocManager.ResolveAsDisposable<IRepository<Leito, long>>())
                {

                    var leitos = _leitoRepositorio.Object.GetAll().AsNoTracking();
                    var uoIds = await leitos.Select(x => x.UnidadeOrganizacionalId).ToListAsync().ConfigureAwait(false);

                    // get com filtro
                    var query = from p in _unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking()
                                    .Where(u => uoIds.Contains(u.Id))
                                    .WhereIf(dropdownInput.filtro == "inter", m => m.IsInternacao == true)
                                    .WhereIf(dropdownInput.filtro == "ambEmr", m => m.IsAmbulatorioEmergencia == true)
                                    .WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(),
                                        m => m.Codigo.Contains(dropdownInput.search)
                                             || m.Descricao.Contains(dropdownInput.search))
                                orderby p.Descricao ascending
                                select new DropdownItems<long>
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    // paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList<long>() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownLocalUtilizacao(DropdownInput dropdownInput)
        {

            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            var unidadeOrganizacionaisDtos = new List<UnidadeOrganizacionalDto>();
            try
            {
                using (var _unidadeOrganizacionalRepositorio =
                    this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var query = from p in _unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking()
                                    .Where(uo => uo.IsLocalUtilizacao)

                                    // .WhereIf(dropdownInput.filtro == "inter", m => m.IsInternacao == true)
                                    // .WhereIf(dropdownInput.filtro == "ambEmr", m => m.IsAmbulatorioEmergencia == true)
                                    .WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(),
                                        m => m.Codigo.Contains(dropdownInput.search)
                                             || m.Descricao.Contains(dropdownInput.search))
                                orderby p.Descricao ascending
                                select new DropdownItems<long>
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    // paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList<long>() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex.InnerException);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownCentroCusto(DropdownInput dropdownInput)
        {

            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            var unidadeOrganizacionaisDtos = new List<UnidadeOrganizacionalDto>();
            try
            {
                using (var unidadeOrganizacionalRepositorio =
                    this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                {
                    var query = from p in unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking()

                                    // .Where(uo => uo.IsCentroCusto)
                                    // .WhereIf(dropdownInput.filtro == "inter", m => m.IsInternacao == true)
                                    // .WhereIf(dropdownInput.filtro == "ambEmr", m => m.IsAmbulatorioEmergencia == true)
                                    .WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(),
                                        m => m.Codigo.Contains(dropdownInput.search))
                                orderby p.Descricao ascending
                                select new DropdownItems<long>
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    // paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList<long>() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex.InnerException);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownPorUsuarioTipoAtendimento(DropdownInput dropdownInput)
        {
            using (var unidadeOrganizacionalRepositorio = this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
            using (var userOrganizationUnitRepository = this._iocManager.ResolveAsDisposable<IRepository<UserOrganizationUnit, long>>())
            {
                var organizationUnitIds = userOrganizationUnitRepository.Object.GetAll().AsNoTracking()
                    .Where(w => w.UserId == this.AbpSession.UserId).Select(s => s.OrganizationUnitId).ToList();

                return await this.ListarDropdownLambda(
                           dropdownInput,
                           unidadeOrganizacionalRepositorio.Object,
                           m => (string.IsNullOrEmpty(dropdownInput.search)
                                 || m.Descricao.Contains(dropdownInput.search)
                                 || m.Codigo.Contains(dropdownInput.search))
                                && ((dropdownInput.filtro == "inter" && m.IsInternacao)
                                    || dropdownInput.filtro == "ambEmr" && m.IsAmbulatorioEmergencia
                                    || (dropdownInput.filtro == string.Empty || dropdownInput.filtro == null))
                                && organizationUnitIds.Any(a => a == m.OrganizationUnitId),
                           p => new DropdownItems
                           {
                               id = p.Id,
                               text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao)
                           },
                           o => o.Descricao).ConfigureAwait(false);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdownTodosPorUsuario(DropdownInput dropdownInput)
        {
            using (var unidadeOrganizacionalRepositorio = this._iocManager.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
            using (var userOrganizationUnitRepository = this._iocManager.ResolveAsDisposable<IRepository<UserOrganizationUnit, long>>())
            {
                var organizationUnitIds = userOrganizationUnitRepository.Object.GetAll().AsNoTracking()
                    .Where(w => w.UserId == this.AbpSession.UserId).Select(s => s.OrganizationUnitId).ToList();

                return await this.ListarDropdownLambda(
                           dropdownInput,
                           unidadeOrganizacionalRepositorio.Object,
                           m => (string.IsNullOrEmpty(dropdownInput.search)
                                 || m.Descricao.Contains(dropdownInput.search)
                                 || m.Codigo.Contains(dropdownInput.search))
                                && organizationUnitIds.Any(a => a == m.OrganizationUnitId),
                           p => new DropdownItems
                           {
                               id = p.Id,
                               text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao)
                           },
                           o => o.Descricao).ConfigureAwait(false);
            }
        }


        public async Task<IResultDropdownList<long>> ListarDropdownComAtendimentoPorUsuario(DropdownInput dropdownInput)
        {
            var sql = @"SELECT SisUnidadeOrganizacional.Id as Id, CONCAT(SisUnidadeOrganizacional.Codigo,SisUnidadeOrganizacional.Descricao) as Text FROM SisUnidadeOrganizacional 
                INNER JOIN(
                SELECT DISTINCT(SisUnidadeOrganizacional.Id) FROM SisUnidadeOrganizacional

                INNER JOIN AteLeito ON AteLeito.SisUnidadeOrganizacionalId = SisUnidadeOrganizacional.Id
                INNER JOIN AteAtendimento
                ON (AteAtendimento.AteLeitoId = AteLeito.Id AND AteAtendimento.DataAlta is null AND AteAtendimento.IsAmbulatorioEmergencia = 0 AND AteAtendimento.IsDeleted = 0  AND AteLeito.IsDeleted = 0) 
                OR(AteAtendimento.SisUnidadeOrganizacionalId = SisUnidadeOrganizacional.Id AND AteAtendimento.DataAlta is null AND AteAtendimento.IsAmbulatorioEmergencia = 1 AND AteAtendimento.IsDeleted = 0)
                WHERE SisUnidadeOrganizacional.IsDeleted = 0
                GROUP BY(SisUnidadeOrganizacional.Id) HAVING COUNT(SisUnidadeOrganizacional.Id) > 0)  
                AS SisUnidadeOrganizacionalFiltro ON SisUnidadeOrganizacionalFiltro.id = SisUnidadeOrganizacional.Id";

            using (var conn = new SqlConnection(this.GetConnection()))
            {
                var list = await conn.QueryAsync<DropdownItems>(sql).ConfigureAwait(false);

                return new ResultDropdownList() { Items = list.ToList(), TotalCount = list.Count() };
            }
        }
    }
}
