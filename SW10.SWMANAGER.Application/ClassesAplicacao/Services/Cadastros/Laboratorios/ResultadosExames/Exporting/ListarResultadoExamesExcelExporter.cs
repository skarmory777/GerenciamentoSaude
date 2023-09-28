using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Exporting
{
    public class ListarResultadoExamesExcelExporter : EpPlusExcelExporterBase, IListarResultadoExamesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarResultadoExamesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ResultadoExameIndexDto> ResultadoExameDto)
        {
            return CreateExcelPackage(
                string.Format("ResultadoExames_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ResultadoExames"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Codigo"),
                        L("Descricao"),
                        L("DataColeta"),
                        L("NumeroExame"),
                        L("NomeExame"),
                        L("Informacao")
                    );

                    AddObjects(
                        sheet, 2, ResultadoExameDto,
                        _ => _.Codigo,
                        _ => _.Descricao,
                        _ => _.DataColeta,
                        _ => _.NumeroExame,
                        _ => _.NomeExame,
                        _ => _.UsuarioIncluidoId
                        //_ => _.Informacao == null ? "" : _.Informacao.Descricao
                            );

                });
        }
    }
}
