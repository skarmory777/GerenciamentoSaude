using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios
{
    public class FaturamentoBrasLaboratorioAppService : SWMANAGERAppServiceBase, IFaturamentoBrasLaboratorioAppService
    {
        public async Task<PagedResultDto<FaturamentoBrasLaboratorioDto>> Listar(ListarFaturamentoBrasLaboratoriosInput input)
        {
            var brasLaboratoriosCount = 0;
            List<FaturamentoBrasLaboratorioDto> brasLaboratoriosDtos = new List<FaturamentoBrasLaboratorioDto>();
            try
            {
                using (var _brasLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasLaboratorio, long>>())
                {
                    var query = _brasLaboratorioRepository.Object.GetAll();

                    brasLaboratoriosCount = await query
                        .CountAsync();

                    brasLaboratoriosDtos = (await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync()
                        ).Select(s => FaturamentoBrasLaboratorioDto.Mapear(s))
                         .ToList();

                    return new PagedResultDto<FaturamentoBrasLaboratorioDto>(brasLaboratoriosCount, brasLaboratoriosDtos);
                }

                //brasLaboratoriosDtos = brasLaboratorios
                //    .MapTo<List<FaturamentoBrasLaboratorioDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarOuEditar(FaturamentoBrasLaboratorioDto input)
        {
            try
            {
                using (var brasLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasLaboratorio, long>>())
                {
                    var BrasLaboratorio = FaturamentoBrasLaboratorioDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        await brasLaboratorioRepository.Object.InsertAsync(BrasLaboratorio);
                    }
                    else
                    {
                        await brasLaboratorioRepository.Object.UpdateAsync(BrasLaboratorio);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoBrasLaboratorioDto input)
        {
            try
            {
                using (var brasLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasLaboratorio, long>>())
                {
                    await brasLaboratorioRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoBrasLaboratorioDto> Obter(long id)
        {
            try
            {
                using (var brasLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasLaboratorio, long>>())
                {
                    var query = await brasLaboratorioRepository.Object
                        .GetAll()
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();

                    return FaturamentoBrasLaboratorioDto.Mapear(query);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoBrasLaboratorioDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                using (var brasLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasLaboratorio, long>>())
                {
                    var query = brasLaboratorioRepository.Object.GetAll();

                    var result = await query.FirstOrDefaultAsync();

                    return FaturamentoBrasLaboratorioDto.Mapear(result);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoBrasLaboratoriosInput input)
        {
            try
            {
                var result = await Listar(input);
                var brasLaboratorios = result.Items;
                using (var listarBrasLaboratoriosExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarBrasLaboratoriosExcelExporter>())
                {
                    return listarBrasLaboratoriosExcelExporter.Object.ExportToFile(brasLaboratorios.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoBrasLaboratorioDto> faturamentoItensDto = new List<FaturamentoBrasLaboratorioDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                using (var brasLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasLaboratorio, long>>())
                {
                    var query = from p in brasLaboratorioRepository.Object.GetAll()
                                .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(), m => m.Id.ToString() == dropdownInput.filtro)
                                .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) ||
                                m.Codigo.Contains(dropdownInput.search))
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };

                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    var result = await queryResultPage.ToListAsync();

                    int total = await query.CountAsync();

                    return new ResultDropdownList() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
