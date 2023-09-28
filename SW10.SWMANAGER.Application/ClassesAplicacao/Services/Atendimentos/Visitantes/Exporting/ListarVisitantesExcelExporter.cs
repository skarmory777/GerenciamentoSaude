using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Exporting;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Exporting
{
    public class ListarVisitantesExcelExporter : EpPlusExcelExporterBase, IListarVisitantesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarVisitantesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<VisitanteDto> visitanteDto)
        {
            return CreateExcelPackage(
                string.Format("Visitantes_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Visitantes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Nome")

                    );

                    AddObjects(
                        sheet, 2, visitanteDto,
                        _ => _.Id

                            );
                });
        }
    }
}
