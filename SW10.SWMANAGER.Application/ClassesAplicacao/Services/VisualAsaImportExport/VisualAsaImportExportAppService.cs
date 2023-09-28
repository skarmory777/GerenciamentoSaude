using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VisualAsaImportExportLogs.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VisualAsaImportExportLogs.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.VisualAsaImportExportLogs;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VisualAsaImportExportLogs
{
    public class VisualAsaImportExportLogAppService : SWMANAGERAppServiceBase, IVisualAsaImportExportLogAppService
    {
        private readonly IRepository<VisualAsaImportExportLog, long> _visualAsaImportExportLogRepositorio;
        private readonly IListarVisualAsaImportExportLogExcelExporter _listarVisualAsaImportExportLogExcelExporter;

        public VisualAsaImportExportLogAppService(IRepository<VisualAsaImportExportLog, long> visualAsaImportExportLogRepositorio,
            IListarVisualAsaImportExportLogExcelExporter listarVisualAsaImportExportLogExcelExporter
            )
        {
            _visualAsaImportExportLogRepositorio = visualAsaImportExportLogRepositorio;
            _listarVisualAsaImportExportLogExcelExporter = listarVisualAsaImportExportLogExcelExporter;
        }

        public async Task CriarOuEditar(VisualAsaImportExportLogDto input)
        {
            try
            {
                var visualAsaImportExportLog = input.MapTo<VisualAsaImportExportLog>();
                if (input.Id.Equals(0))
                {
                    await _visualAsaImportExportLogRepositorio.InsertOrUpdateAsync(visualAsaImportExportLog);
                }
                else
                {
                    await _visualAsaImportExportLogRepositorio.UpdateAsync(visualAsaImportExportLog);
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroSalvar"));
            }

        }

        public async Task<PagedResultDto<VisualAsaImportExportLogDto>> Listar(ListarInput input)
        {
            var contarVisualAsaImportExportLogs = 0;
            List<VisualAsaImportExportLog> visualAsaImportExportLogs;
            List<VisualAsaImportExportLogDto> visualAsaImportExportLogsDtos = new List<VisualAsaImportExportLogDto>();
            try
            {
                var query = _visualAsaImportExportLogRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarVisualAsaImportExportLogs = await query
                    .CountAsync();

                visualAsaImportExportLogs = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                visualAsaImportExportLogsDtos = visualAsaImportExportLogs
                    .MapTo<List<VisualAsaImportExportLogDto>>();

                return new PagedResultDto<VisualAsaImportExportLogDto>(
                    contarVisualAsaImportExportLogs,
                    visualAsaImportExportLogsDtos
                    );

            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarInput input)
        {
            try
            {
                var query = await Listar(input);

                var visualAsaImportExportLogsDtos = query.Items;

                return _listarVisualAsaImportExportLogExcelExporter.ExportToFile(visualAsaImportExportLogsDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<VisualAsaImportExportLogDto> Obter(long id)
        {
            try
            {
                var result = await _visualAsaImportExportLogRepositorio.GetAsync(id);
                var visualAsaImportExportLog = result.MapTo<VisualAsaImportExportLogDto>();
                return visualAsaImportExportLog;
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }

        }
    }
}
