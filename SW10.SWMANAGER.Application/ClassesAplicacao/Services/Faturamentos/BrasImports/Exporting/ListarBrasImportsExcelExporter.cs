using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Exporting
{
    public class ListarBrasImportsExcelExporter : EpPlusExcelExporterBase, IListarBrasImportsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarBrasImportsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FaturamentoBrasImportDto> BrasImportDto)
        {
            return CreateExcelPackage(
                string.Format("BrasImports_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BrasImports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CodigoApresentacao")
                    );

                    AddObjects(
                        sheet, 2, BrasImportDto,
                        _ => _.CodigoApresentacao
                            );

                });
        }
    }
}
