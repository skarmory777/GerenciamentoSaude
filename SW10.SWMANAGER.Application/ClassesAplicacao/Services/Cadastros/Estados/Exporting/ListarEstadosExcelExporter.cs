using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Exporting
{
    public class ListarEstadosExcelExporter : EpPlusExcelExporterBase, IListarEstadosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarEstadosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<EstadoDto> estadoDto)
        {
            return CreateExcelPackage(
                string.Format("Estados_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Estados"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Nome"),
                        L("Uf")
                    );

                    AddObjects(
                        sheet, 2, estadoDto,
                        _ => _.Nome,
                        _ => _.Uf
                            );

                });
        }
    }
}
