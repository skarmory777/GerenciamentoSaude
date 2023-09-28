using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Exporting
{
    public class ListarAssistencialAtendimentosExcelExporter : EpPlusExcelExporterBase, IListarAssistencialAtendimentosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarAssistencialAtendimentosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<AtendimentoIndexDto> AssistencialAtendimentosDto)
        {
            return CreateExcelPackage(
                string.Format("AssistencialAtendimentos_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AssistencialAtendimentos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Codigo")
                    );

                    AddObjects(
                        sheet, 2, AssistencialAtendimentosDto,
                        _ => _.Id,
                        _ => _.Codigo
                    );

                });
        }
    }
}
