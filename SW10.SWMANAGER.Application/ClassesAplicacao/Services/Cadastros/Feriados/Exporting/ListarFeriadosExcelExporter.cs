using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Exporting
{
    public class ListarFeriadosExcelExporter : EpPlusExcelExporterBase, IListarFeriadosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarFeriadosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FeriadoDto> feriadoDto)
        {
            return CreateExcelPackage(
                string.Format("Feriados_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Feriados"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DiaMesAno"),
                        L("Descricao")
                    );

                    AddObjects(
                        sheet, 2, feriadoDto,
                        _ => _.DiaMesAno,
                        _ => _.Descricao
                            );

                });
        }
    }
}
