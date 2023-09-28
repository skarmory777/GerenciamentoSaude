using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas
{
    public class EmpresaAppService : SWMANAGERAppServiceBase, IEmpresaAppService
    {
        private readonly IRepository<Empresa, long> _empresaRepository;
        private readonly IListarEmpresasExcelExporter _listarEmpresasExcelExporter;
        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;
        private readonly IRepository<UserEmpresa, long> _userEmpresas;

        public EmpresaAppService(
            IRepository<Empresa, long> empresaRepository,
            IListarEmpresasExcelExporter listarEmpresasExcelExporter
            ,
            UserManager userManager
            ,
            IUserAppService userAppService,
             IRepository<UserEmpresa, long> userEmpresas
            )
        {
            _empresaRepository = empresaRepository;
            _listarEmpresasExcelExporter = listarEmpresasExcelExporter;
            _userManager = userManager;
            _userAppService = userAppService;
            _userEmpresas = userEmpresas;
        }

        public async Task CriarOuEditar(EmpresaDto input)
        {
            try
            {
                var empresa = input.MapTo<Empresa>();
                if (input.Id.Equals(0))
                {
                    await _empresaRepository.InsertAsync(empresa);
                }
                else
                {
                    var ori = await _empresaRepository.GetAsync(empresa.Id);
                    ori.Cnes = empresa.Cnes;
                    ori.Codigo = empresa.Codigo;
                    ori.CodigoSus = empresa.CodigoSus;
                    ori.Convenio = empresa.Convenio;
                    ori.Descricao = empresa.Descricao;
                    ori.EstoqueId = empresa.EstoqueId;
                    ori.IsAtiva = empresa.IsAtiva;
                    ori.IsComprasUnificadas = empresa.IsComprasUnificadas;
                    ori.Logotipo = empresa.Logotipo;
                    ori.LogotipoMimeType = empresa.LogotipoMimeType;
                    ori.RazaoSocial = empresa.RazaoSocial;

                    await _empresaRepository.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(EmpresaDto input)
        {
            try
            {
                await _empresaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<EmpresaDto>> Listar(ListarEmpresasInput input)
        {
            var contarEmpresas = 0;
            List<Empresa> empresas;
            var empresasDtos = new List<EmpresaDto>();
            try
            {
                var query = _empresaRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.NomeFantasia.Contains(input.Filtro) ||
                        m.RazaoSocial.Contains(input.Filtro) ||
                        m.Cnpj.Contains(input.Filtro) ||
                        m.InscricaoEstadual.Contains(input.Filtro) ||
                        m.InscricaoMunicipal.Contains(input.Filtro) ||
                        m.Telefone1.Contains(input.Filtro) ||
                        m.Telefone2.Contains(input.Filtro) ||
                        m.Telefone3.Contains(input.Filtro) ||
                        m.Telefone4.Contains(input.Filtro) ||
                        m.Email.Contains(input.Filtro) ||
                        m.Logradouro.Contains(input.Filtro) ||
                        m.Bairro.Contains(input.Filtro) ||
                        m.Cidade.Nome.Contains(input.Filtro) ||
                        m.Estado.Nome.Contains(input.Filtro) ||
                        m.Estado.Uf.Contains(input.Filtro)
                    );

                contarEmpresas = await query
                    .CountAsync();

                empresas = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                empresasDtos = empresas
                    .MapTo<List<EmpresaDto>>();

                return new PagedResultDto<EmpresaDto>(
                    contarEmpresas,
                    empresasDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<EmpresaDto>> ListarTodos()
        {
            try
            {
                var query = await _empresaRepository
                    .GetAllListAsync();

                var empresas = query.MapTo<List<EmpresaDto>>();
                return new ListResultDto<EmpresaDto> { Items = empresas };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarEmpresasInput input)
        {
            try
            {
                var result = await Listar(input);
                var empresas = result.Items;
                return _listarEmpresasExcelExporter.ExportToFile(empresas.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<EmpresaDto> Obter(long id)
        {
            try
            {
                var result = await _empresaRepository
                    .GetAllListAsync(m => m.Id == id);

                var empresa = result
                    .FirstOrDefault()
                    .MapTo<EmpresaDto>();

                return empresa;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _empresaRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(dropdownInput.search) ||
                        m.NomeFantasia
                        .Contains(dropdownInput.search)
                        )
                            orderby p.NomeFantasia ascending
                            select new DropdownItems { id = p.Id, text = p.NomeFantasia };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdownPorUsuario(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                // Filtrando por usuario
                var user = await _userManager.GetUserByIdAsync((long)AbpSession.UserId);
                var empresas = await _userAppService.GetUserEmpresas(AbpSession.UserId.Value);
                var empresasDto = empresas.Items.ToList();

                //get com filtro
                var query = from p in empresasDto
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(dropdownInput.search) ||
                        m.NomeFantasia.Contains(dropdownInput.search))
                            orderby p.NomeFantasia ascending
                            select new DropdownItems { id = p.Id, text = p.NomeFantasia };

                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = queryResultPage.Count() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex.InnerException);
            }
        }
    }
}
