using Abp.Runtime.Session;
using Abp.Timing.Timezone;

using SW10.SWMANAGER.DataExporting.Excel.EpPlus;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Exporting
{
    public class ListarResultadoLaudosExcelExporter : EpPlusExcelExporterBase, IListarResultadoLaudosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarResultadoLaudosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        //public FileDto ExportToFile(List<ResultadoLaudoDto> ResultadoLaudoDto)
        //{
        //    return CreateExcelPackage(
        //        string.Format("ResultadoLaudos_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
        //        excelPackage =>
        //        {
        //            var sheet = excelPackage.Workbook.Worksheets.Add(L("ResultadoLaudos"));
        //            sheet.OutLineApplyStyle = true;

        //            AddHeader(
        //                sheet,
        //                L("Codigo"),
        //                L("Descricao"),
        //                L("TipoLayout"),
        //                L("DiretorioOrdem"),
        //                L("DiretorioResultado"),
        //                L("Informacao")
        //            );

        //            AddObjects(
        //                sheet, 2, ResultadoLaudoDto,
        //                _ => _.Codigo,
        //                _ => _.Descricao,
        //                _ => _.TipoLayout,
        //                _ => _.DiretorioOrdem,
        //                _ => _.DiretorioResultado
        //                //_ => _.Informacao == null ? "" : _.Informacao.Descricao
        //                    );

        //        });
        //}
    }
}
