using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos
{
    public class CentroCustoAppService : SWMANAGERAppServiceBase, ICentroCustoAppService
    {
        private readonly IRepository<CentroCusto, long> _centroCustoRepository;
        private readonly IListarCentrosCustosExcelExporter _listarCentrosCustosExcelExporter;
        private readonly IRepository<ContaAdministrativaCentroCusto, long> _contaAdministrativaCentroCustoRepository;

        public CentroCustoAppService(
            IRepository<CentroCusto, long> centroCustoRepository,
            IListarCentrosCustosExcelExporter listarCentrosCustosExcelExporter,
            IRepository<ContaAdministrativaCentroCusto, long> contaAdministrativaCentroCustoRepository
            )
        {
            _centroCustoRepository = centroCustoRepository;
            _listarCentrosCustosExcelExporter = listarCentrosCustosExcelExporter;
            _contaAdministrativaCentroCustoRepository = contaAdministrativaCentroCustoRepository;
        }

        public async Task CriarOuEditar(CriarOuEditarCentroCusto input)
        {
            try
            {
                var centroCusto = input.MapTo<CentroCusto>();
                if (input.Id.Equals(0))
                {
                    await _centroCustoRepository.InsertAsync(centroCusto);
                }
                else
                {
                    await _centroCustoRepository.UpdateAsync(centroCusto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarCentroCusto input)
        {
            try
            {
                await _centroCustoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<CentroCustoDto>> Listar(ListarCentrosCustosInput input)
        {
            var contarCentrosCustos = 0;
            List<CentroCusto> centrosCustos;
            List<CentroCustoDto> centrosCustosDtos = new List<CentroCustoDto>();
            try
            {
                var query = _centroCustoRepository
                    .GetAll()
                    .Include(m => m.GrupoCentroCusto)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarCentrosCustos = await query
                    .CountAsync();

                centrosCustos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                centrosCustosDtos = centrosCustos
                    .MapTo<List<CentroCustoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<CentroCustoDto>(
                contarCentrosCustos,
                centrosCustosDtos
                );
        }

        public async Task<ListResultDto<CentroCustoDto>> ListarTodos()
        {
            List<CentroCusto> centrosCustos;
            List<CentroCustoDto> centrosCustosDtos = new List<CentroCustoDto>();
            try
            {
                centrosCustos = await _centroCustoRepository
                  .GetAll()
                  .Include(m => m.GrupoCentroCusto)
                  .AsNoTracking()
                  .ToListAsync();

                centrosCustosDtos = centrosCustos
                    .MapTo<List<CentroCustoDto>>();

                return new ListResultDto<CentroCustoDto> { Items = centrosCustosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _centroCustoRepository
                    .GetAll()
                    .Include(m => m.GrupoCentroCusto)
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    ).Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();



                return new ListResultDto<GenericoIdNome> { Items = query };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarCentrosCustosInput input)
        {
            try
            {
                var result = await Listar(input);
                var origens = result.Items;
                return _listarCentrosCustosExcelExporter.ExportToFile(origens.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarCentroCusto> Obter(long id)
        {
            try
            {
                var result = await _centroCustoRepository
                    .GetAll()
                    .Include(m => m.GrupoCentroCusto)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();
                var centroCusto = result.MapTo<CriarOuEditarCentroCusto>();
                return centroCusto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<CentroCustoDto> centrosCustoDto = new List<CentroCustoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _centroCustoRepository.GetAll()
                            .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.Id.ToString() == dropdownInput.filtro)
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.Descricao ascending
                            select new DropdownItems<long>
                            {
                                id = p.Id,
                                text = string.Concat(p.CodigoCentroCusto, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList<long>() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdownCodigoCentroCusto(DropdownInput dropdownInput)
        {
            return await ListarDropdownLambda(dropdownInput
                                                  , _centroCustoRepository
                                                  , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                 || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())) && m.IsAtivo.Equals(true)


                                                 , p => new DropdownItems { id = p.Id, text = string.Concat(p.CodigoCentroCusto.ToString(), " - ", p.Descricao) }
                                                 , o => o.Descricao
                                                 );
        }

        public async Task<IResultDropdownList<long>> ListarDropdownCodigoCentroCustoReceberLancamento(DropdownInput dropdownInput)
        {
            return await ListarDropdownLambda(dropdownInput
                                                  , _centroCustoRepository
                                                  , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                 || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())) && m.IsAtivo.Equals(true) && m.IsReceberLancamento.Equals(true)


                                                 , p => new DropdownItems { id = p.Id, text = string.Concat(p.CodigoCentroCusto.ToString(), " - ", p.Descricao) }
                                                 , o => o.Descricao
                                                 );
        }

        public async Task<IResultDropdownList<long>> ListarDropdownCodigoCentroCustoPorContaAdministrativa(DropdownInput dropdownInput)
        {
            long contaAdministrativaId;

            long.TryParse(dropdownInput.filtro, out contaAdministrativaId);


            return await ListarDropdownLambda(dropdownInput
                                                  , _contaAdministrativaCentroCustoRepository
                                                  , m =>
                                                  ((string.IsNullOrEmpty(dropdownInput.search) || m.CentroCusto.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                  || m.CentroCusto.CodigoCentroCusto.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                                                  )

                                                 && ((ContaAdministrativaCentroCusto)m).ContaAdministrativaId == contaAdministrativaId

                                                 )

                                                 , p => new DropdownItems { id = p.CentroCustoId, text = string.Concat(p.CentroCusto.CodigoCentroCusto.ToString(), " - ", p.CentroCusto.Descricao) }
                                                 , o => o.CentroCusto.Descricao
                                                 );
        }


    }
}
