using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Exporting
{
    public class ListarAtestadosExcelExporter : EpPlusExcelExporterBase, IListarAtestadosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarAtestadosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<AtestadoDto> atestadoDto)
        {
            return CreateExcelPackage(
                string.Format("Estados_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Estados"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Conteudo")
                    );

                    AddObjects(
                        sheet, 2, atestadoDto,
                        _ => _.Id,
                        _ => _.Conteudo
                            );

                });
        }
    }
}
