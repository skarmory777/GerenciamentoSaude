using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Exporting
{
    public class ListarFaturamentoTabelasExcelExporter : EpPlusExcelExporterBase, IListarFaturamentoTabelasExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarFaturamentoTabelasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FaturamentoTabelaDto> TabelaDto)
        {
            return CreateExcelPackage(
                string.Format("Tabelas_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Tabelas"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Descricao")
                    );

                    AddObjects(
                        sheet, 2, TabelaDto,
                        _ => _.Descricao
                            );

                });
        }
    }
}
