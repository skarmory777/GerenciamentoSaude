using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Exporting
{
    public class ListarClassificacoesRiscoExcelExporter : EpPlusExcelExporterBase, IListarClassificacoesRiscoExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarClassificacoesRiscoExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ClassificacaoRiscoDto> classificacoesRiscoDto)
        {
            return CreateExcelPackage(
                string.Format("ClassificacoesRisco_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ClassificacoesRisco"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Descricao")

                    );

                    AddObjects(
                        sheet, 2, classificacoesRiscoDto,
                        _ => _.Id
                        //,
                        //_ => _.Descricao
                            );
                });
        }
    }
}
