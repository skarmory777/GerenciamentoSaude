using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Exporting
{
    public class ListarConsultorTabelaCamposExcelExporter : EpPlusExcelExporterBase, IListarConsultorTabelaCamposExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarConsultorTabelaCamposExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ConsultorTabelaCampoDto> consultorTabelasDto)
        {
            return CreateExcelPackage(
                string.Format("ConsultorTabelas_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ConsultorTabelas"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Campo")
                    );

                    AddObjects(
                        sheet, 2, consultorTabelasDto,
                        _ => _.Id,
                        _ => _.Descricao
                            );

                    //Formatting cells

                    //var timeColumn1 = sheet.Column(4);

                    //timeColumn1.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                });
        }
    }
}
