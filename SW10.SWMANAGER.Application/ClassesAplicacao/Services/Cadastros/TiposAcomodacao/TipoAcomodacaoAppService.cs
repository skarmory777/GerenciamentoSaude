using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class TipoAcomodacaoAppService : SWMANAGERAppServiceBase, ITipoAcomodacaoAppService
    {
        private readonly IRepository<TipoAcomodacao, long> _tipoAcomodacaoRepositorio;
        private readonly IListarTipoAcomodacaoExcelExporter _listarTipoAcomodacaoExcelExporter;
        private readonly IRepository<Leito, long> _leitoRepositorio;

        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<UnidadeOrganizacional, long> _unidadeOrganizacionalRepositorio;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepositorio;

        public TipoAcomodacaoAppService(IRepository<TipoAcomodacao, long> tipoAcomodacaoRepositorio,
            IListarTipoAcomodacaoExcelExporter listarTipoAcomodacaoExcelExporter,
            IRepository<Leito, long> leitoRepositorio,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<UnidadeOrganizacional, long> unidadeOrganizacionalRepositorio,
            IRepository<OrganizationUnit, long> organizationUnitRepositorio)
        {
            _tipoAcomodacaoRepositorio = tipoAcomodacaoRepositorio;
            _listarTipoAcomodacaoExcelExporter = listarTipoAcomodacaoExcelExporter;
            _leitoRepositorio = leitoRepositorio;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _unidadeOrganizacionalRepositorio = unidadeOrganizacionalRepositorio;
            _organizationUnitRepositorio = organizationUnitRepositorio;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Edit)]
        public async Task CriarOuEditar(TipoAcomodacaoDto input)
        {
            try
            {
                var tipoAcomodacao = input.MapTo<TipoAcomodacao>();
                if (input.Id.Equals(0))
                {
                    await this._tipoAcomodacaoRepositorio.InsertOrUpdateAsync(tipoAcomodacao).ConfigureAwait(false);
                }
                else
                {
                    var tipoAcomodacaoEntity = _tipoAcomodacaoRepositorio.GetAll()
                                                                         .Where(w => w.Id == input.Id)
                                                                         .FirstOrDefault();

                    if (tipoAcomodacaoEntity != null)
                    {
                        tipoAcomodacaoEntity.Codigo = input.Codigo;
                        tipoAcomodacaoEntity.Descricao = input.Descricao;
                        tipoAcomodacaoEntity.TabelaItemTissId = input.TabelaItemTissId;

                        await this._tipoAcomodacaoRepositorio.UpdateAsync(tipoAcomodacaoEntity).ConfigureAwait(false);
                    }

                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroSalvar"));
            }

        }

        public async Task Excluir(TipoAcomodacaoDto input)
        {
            try
            {
                await this._tipoAcomodacaoRepositorio.DeleteAsync(input.Id).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExcluir"));
            }

        }

        public async Task<PagedResultDto<TipoAcomodacaoDto>> Listar(ListarTiposAcomodacaoInput input)
        {
            var contarTiposAcomodacao = 0;
            List<TipoAcomodacao> tiposAcomodacao;
            List<TipoAcomodacaoDto> tiposAcomodacaoDtos = new List<TipoAcomodacaoDto>();
            try
            {
                var query = _tipoAcomodacaoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposAcomodacao = await query
                                            .CountAsync().ConfigureAwait(false);

                tiposAcomodacao = await query
                                      .AsNoTracking()
                                      .OrderBy(input.Sorting)
                                      .PageBy(input)
                                      .ToListAsync().ConfigureAwait(false);

                tiposAcomodacaoDtos = tiposAcomodacao
                    .MapTo<List<TipoAcomodacaoDto>>();

                return new PagedResultDto<TipoAcomodacaoDto>(
                    contarTiposAcomodacao,
                    tiposAcomodacaoDtos
                    );

            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposAcomodacaoInput input)
        {
            try
            {
                var query = await this.Listar(input).ConfigureAwait(false);

                var tiposAcomodacaoDtos = query.Items;

                return _listarTipoAcomodacaoExcelExporter.ExportToFile(tiposAcomodacaoDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<TipoAcomodacaoDto> Obter(long id)
        {
            try
            {
                var result = await this._tipoAcomodacaoRepositorio.GetAsync(id).ConfigureAwait(false);
                var tipoAcomodacao = result.MapTo<TipoAcomodacaoDto>();
                return tipoAcomodacao;
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._tipoAcomodacaoRepositorio).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }



        public async Task<PagedResultDto<TipoAcomodacaoDto>> ListarComLeito(ListarTiposAcomodacaoInput input)
        {
            var contarTiposAcomodacao = 0;
            List<TipoAcomodacao> tiposAcomodacao;
            List<TipoAcomodacaoDto> tiposAcomodacaoDtos = new List<TipoAcomodacaoDto>();
            try
            {

                var unidades = from userOrg in _userOrganizationUnitRepository.GetAll()
                               join org in _unidadeOrganizacionalRepositorio.GetAll()
                               on userOrg.OrganizationUnitId equals org.Id

                               join orgUnit in _organizationUnitRepositorio.GetAll()
                               on org.OrganizationUnitId equals orgUnit.Id

                               where userOrg.UserId == AbpSession.UserId
                               select orgUnit;



                var queryLeito = _leitoRepositorio.GetAll()
                                                  .Where(w => unidades.Any(a => w.UnidadeOrganizacional.OrganizationUnit.Code.StartsWith(a.Code)))
                                                  ;


                var query = _tipoAcomodacaoRepositorio
                    .GetAll()

                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    )
                    .Where(m => queryLeito.Any(a => a.TipoAcomodacaoId == m.Id));

                contarTiposAcomodacao = await query
                                            .CountAsync().ConfigureAwait(false);

                tiposAcomodacao = await query
                                      .AsNoTracking()
                                      .OrderBy(input.Sorting)
                                      .PageBy(input)
                                      .ToListAsync().ConfigureAwait(false);

                tiposAcomodacaoDtos = tiposAcomodacao
                    .MapTo<List<TipoAcomodacaoDto>>();

                return new PagedResultDto<TipoAcomodacaoDto>(
                    contarTiposAcomodacao,
                    tiposAcomodacaoDtos
                    );

            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }



    }
}
