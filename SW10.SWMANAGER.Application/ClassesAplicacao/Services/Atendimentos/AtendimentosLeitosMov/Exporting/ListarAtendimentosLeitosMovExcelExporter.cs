using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Exporting
{
    public class ListarAtendimentosLeitosMovExcelExporter : EpPlusExcelExporterBase, IListarAtendimentosLeitosMovExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarAtendimentosLeitosMovExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<AtendimentoLeitoMovDto> visitanteDto)
        {
            return CreateExcelPackage(
                string.Format("AtendimentosLeitosMov_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AtendimentosLeitosMov"));
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
