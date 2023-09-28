using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Exporting
{
    public class ListarContasExcelExporter : EpPlusExcelExporterBase, IListarContasExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarContasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FaturamentoContaDto> ContaDto)
        {
            return CreateExcelPackage(
                string.Format("Contas_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Contas"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Titular")
                    );

                    AddObjects(
                        sheet, 2, ContaDto,
                        _ => _.Titular
                            );

                });
        }
    }
}
