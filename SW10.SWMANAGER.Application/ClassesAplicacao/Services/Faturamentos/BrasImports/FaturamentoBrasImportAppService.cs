using Abp.Application.Services.Dto;
using Abp.Dependency;
//using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasImports;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports
{
    public class FaturamentoBrasImportAppService : SWMANAGERAppServiceBase, IFaturamentoBrasImportAppService
    {
        public async Task<PagedResultDto<FaturamentoBrasImportDto>> Listar(ListarBrasImportsInput input)
        {
            var brasImportrBrasImports = 0;
            List<FaturamentoBrasImportDto> brasImportsDtos = new List<FaturamentoBrasImportDto>();
            try
            {
                using (var brasImportRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasImport, long>>())
                {

                    var query = brasImportRepository.Object
                        .GetAll();

                    brasImportrBrasImports = await query.CountAsync();

                    brasImportsDtos = (await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync()
                        )
                        .Select(s => FaturamentoBrasImportDto.Mapear(s))
                        .ToList()
                        ;

                    // brasImportsDtos = brasImports.MapTo<List<FaturamentoBrasImportDto>>();

                    return new PagedResultDto<FaturamentoBrasImportDto>(
                       brasImportrBrasImports,
                       brasImportsDtos
                    );
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarOuEditar(FaturamentoBrasImportDto input)
        {
            try
            {
                var BrasImport = FaturamentoBrasImportDto.Mapear(input);
                using (var brasImportRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasImport, long>>())
                {
                    if (input.Id.Equals(0))
                    {
                        await brasImportRepository.Object.InsertAsync(BrasImport);
                    }
                    else
                    {
                        await brasImportRepository.Object.UpdateAsync(BrasImport);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoBrasImportDto input)
        {
            try
            {
                using (var brasImportRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasImport, long>>())
                {
                    await brasImportRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoBrasImportDto> Obter(long id)
        {
            try
            {
                using (var brasImportRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasImport, long>>())
                {
                    var query = await brasImportRepository.Object
                        .GetAll()
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();

                    var brasImport = FaturamentoBrasImportDto.Mapear(query);

                    return brasImport;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FaturamentoBrasImportDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                using (var brasImportRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoBrasImport, long>>())
                {
                    var query = brasImportRepository.Object.GetAll();

                    var result = await query.FirstOrDefaultAsync();

                    var brasImport = FaturamentoBrasImportDto.Mapear(result);

                    return brasImport;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarBrasImportsInput input)
        {
            try
            {
                var result = await Listar(input);
                var brasImports = result.Items;
                using (var listarBrasImportsExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarBrasImportsExcelExporter>())
                {
                    return listarBrasImportsExcelExporter.Object.ExportToFile(brasImports.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }
    }
}
