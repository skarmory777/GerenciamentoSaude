using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Exporting
{
    public class ListarBrasPrecosExcelExporter : EpPlusExcelExporterBase, IListarBrasPrecosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarBrasPrecosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FaturamentoBrasPrecoDto> BrasPrecoDto)
        {
            return CreateExcelPackage(
                string.Format("BrasPrecos_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BrasPrecos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CodigoBrasTiss")
                    );

                    AddObjects(
                        sheet, 2, BrasPrecoDto,
                        _ => _.CodigoBrasTiss
                            );

                });
        }
    }
}
