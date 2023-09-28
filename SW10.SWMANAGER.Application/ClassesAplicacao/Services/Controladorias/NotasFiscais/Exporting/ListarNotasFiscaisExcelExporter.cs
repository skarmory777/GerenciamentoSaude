using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NotasFiscais.Exporting
{
    public class ListarNotasFiscaisExcelExporter : EpPlusExcelExporterBase, IListarNotasFiscaisExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarNotasFiscaisExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        //public FileDto ExportToFile(List<NotaFiscalDto> notasFiscaisDto)
        //{
        //    return CreateExcelPackage(
        //        string.Format("NotasFiscais_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
        //        excelPackage =>
        //        {
        //            var sheet = excelPackage.Workbook.Worksheets.Add(L("NotasFiscais"));
        //            sheet.OutLineApplyStyle = true;

        //            AddHeader(
        //                sheet,
        //                L("Numero"),
        //                L("ChaveAcesso")
        //            );

        //            AddObjects(
        //                sheet, 2, notasFiscaisDto,
        //                _ => _.Nsu,
        //                _ => _.ChaveAcesso
        //                    );

        //            //Formatting cells

        //            //var timeColumn1 = sheet.Column(4);

        //            //timeColumn1.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
        //        });
        //}
    }
}
