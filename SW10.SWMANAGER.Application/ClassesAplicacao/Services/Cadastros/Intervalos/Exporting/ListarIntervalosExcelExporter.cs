using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Exporting
{
    public class ListarIntervalosExcelExporter : EpPlusExcelExporterBase, IListarIntervalosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarIntervalosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<IntervaloDto> intervalosDto)
        {
            return CreateExcelPackage(
                string.Format("Intervalos_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Intervalos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("NomeFantasia"),
                        L("IntervaloMinutos"),
                        L("AtendimentosPorHora")
                    );

                    AddObjects(
                        sheet, 2, intervalosDto,
                        _ => _.Id,
                        _ => _.Nome,
                        _ => _.IntervaloMinutos,
                        _ => _.AtendimentosPorHora
                            );
                });
        }
    }
}
