using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Exporting
{
    public class ListarResultadosExcelExporter : EpPlusExcelExporterBase, IListarResultadosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarResultadosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ResultadoIndexDto> ResultadoDto)
        {
            return CreateExcelPackage(
                string.Format("Resultados_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Resultados"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DataColeta"),
                        L("Tecnico"),
                        L("MedicoSolicitante"),
                        L("DataEntrega"),
                        L("EntreguePor")
                    );

                    AddObjects(
                        sheet, 2, ResultadoDto,
                        _ => _.DataColeta,
                        _ => _.Tecnico,
                        _ => _.MedicoSolicitante,
                        _ => _.DataEntrega,
                        _ => _.EntreguePor
                            );

                });
        }
    }
}
