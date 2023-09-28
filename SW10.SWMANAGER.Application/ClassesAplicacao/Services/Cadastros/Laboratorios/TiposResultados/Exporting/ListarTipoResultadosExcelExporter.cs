using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Exporting
{
    public class ListarTipoResultadosExcelExporter : EpPlusExcelExporterBase, IListarTipoResultadosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarTipoResultadosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<TipoResultadoDto> TipoResultadoDto)
        {
            return CreateExcelPackage(
                string.Format("TipoResultados_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TipoResultados"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Codigo"),
                        L("Descricao")
                    );

                    AddObjects(
                        sheet, 2, TipoResultadoDto,
                        _ => _.Codigo,
                        _ => _.Descricao
                            );

                });
        }
    }
}
