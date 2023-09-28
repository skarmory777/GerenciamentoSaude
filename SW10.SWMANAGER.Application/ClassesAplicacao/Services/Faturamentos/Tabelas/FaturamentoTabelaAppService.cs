using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Tabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas
{
    public class FaturamentoTabelaAppService : SWMANAGERAppServiceBase, IFaturamentoTabelaAppService
    {
        private readonly IRepository<FaturamentoTabela, long> _tabelaRepository;
        private readonly IListarFaturamentoTabelasExcelExporter _listarTabelasExcelExporter;

        public FaturamentoTabelaAppService(IRepository<FaturamentoTabela, long> tabelaRepository, IListarFaturamentoTabelasExcelExporter listarTabelasExcelExporter)
        {
            _tabelaRepository = tabelaRepository;
            _listarTabelasExcelExporter = listarTabelasExcelExporter;
        }

        public async Task<PagedResultDto<FaturamentoTabelaDto>> Listar(ListarFaturamentoTabelasInput input)
        {
            var tabelarTabelas = 0;
            List<FaturamentoTabelaDto> tabelasDtos = new List<FaturamentoTabelaDto>();
            try
            {
                var query = _tabelaRepository
                    .GetAll()
                    .WhereIf(!string.IsNullOrEmpty(input.Filtro), m => m.Descricao.Contains(input.Filtro));

                tabelarTabelas = await query
                    .CountAsync();

                tabelasDtos = (await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync())
                    .Select(s => FaturamentoTabelaDto.Mapear(s))
                    .ToList();                
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoTabelaDto>(
                tabelarTabelas,
                tabelasDtos
                );
        }

        public async Task<long?> CriarOuEditar(FaturamentoTabelaDto input)
        {
            try
            {
                var Tabela = FaturamentoTabelaDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    return await _tabelaRepository.InsertAndGetIdAsync(Tabela);
                }
                else
                {
                    var tabela = await _tabelaRepository.UpdateAsync(Tabela);
                    var tabelaDto = FaturamentoTabelaDto.Mapear(tabela);
                    return tabelaDto.Id;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoTabelaDto input)
        {
            try
            {
                await _tabelaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoTabelaDto> Obter(long id)
        {
            try
            {
                var query = await _tabelaRepository
                    .GetAll()
                    //     .Include(m => m.Estado)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                return FaturamentoTabelaDto.Mapear(query);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoTabelaDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                var query = _tabelaRepository
                    .GetAll();

                var result = await query.FirstOrDefaultAsync();

                return FaturamentoTabelaDto.Mapear(result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoTabelasInput input)
        {
            try
            {
                var result = await Listar(input);
                var tabelas = result.Items;
                return _listarTabelasExcelExporter.ExportToFile(tabelas.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _tabelaRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                     || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
