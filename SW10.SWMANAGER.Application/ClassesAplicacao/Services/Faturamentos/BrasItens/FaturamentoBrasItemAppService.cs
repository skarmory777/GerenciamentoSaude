#region Usings
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens
{
    public class FaturamentoBrasItemAppService : SWMANAGERAppServiceBase, IFaturamentoBrasItemAppService
    {

        public async Task<PagedResultDto<FaturamentoBrasItemDto>> Listar(ListarFaturamentoBrasItensInput input)
        {
            var brasItemrBrasItens = 0;
            List<FaturamentoBrasItemDto> brasItensDtos = new List<FaturamentoBrasItemDto>();
            try
            {
                using (var brasItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasItem, long>>())
                {
                    var query = brasItemRepository.Object.GetAll();

                    brasItemrBrasItens = await query
                        .CountAsync();

                    brasItensDtos = (await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync()
                        ).Select(s => FaturamentoBrasItemDto.Mapear(s))
                        .ToList();
                }
                //brasItensDtos = brasItens
                //    .MapTo<List<FaturamentoBrasItemDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoBrasItemDto>(
                brasItemrBrasItens,
                brasItensDtos
                );
        }

        public async Task CriarOuEditar(FaturamentoBrasItemDto input)
        {
            try
            {
                using (var brasItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasItem, long>>())
                {
                    var BrasItem = FaturamentoBrasItemDto.Mapear(input);//.MapTo<FaturamentoBrasItem>();
                    if (input.Id.Equals(0))
                    {
                        await brasItemRepository.Object.InsertAsync(BrasItem);
                    }
                    else
                    {
                        await brasItemRepository.Object.UpdateAsync(BrasItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoBrasItemDto input)
        {
            try
            {
                using (var brasItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasItem, long>>())
                {
                    await brasItemRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoBrasItemDto> Obter(long id)
        {
            try
            {
                using (var brasItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasItem, long>>())
                {
                    var query = await brasItemRepository.Object
                        .GetAll()
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();

                    return FaturamentoBrasItemDto.Mapear(query);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoBrasItemDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                using (var brasItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasItem, long>>())
                {
                    var query = brasItemRepository.Object.GetAll();
                    var result = await query.FirstOrDefaultAsync();

                    //var brasItem = result
                    //    .MapTo<FaturamentoBrasItemDto>();

                    return FaturamentoBrasItemDto.Mapear(result);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoBrasItensInput input)
        {
            try
            {
                var result = await Listar(input);
                var brasItens = result.Items;
                using (var listarBrasItensExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarBrasItensExcelExporter>())
                {
                    return listarBrasItensExcelExporter.Object.ExportToFile(brasItens.ToList());
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

            List<FaturamentoBrasItemDto> faturamentoItensDto = new List<FaturamentoBrasItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }
                using (var brasItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasItem, long>>())
                {

                    var query = from p in brasItemRepository.Object.GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.Contains(dropdownInput.search))
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
