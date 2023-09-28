using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Exporting
{
    public class ListarItensTabelaExcelExporter : EpPlusExcelExporterBase, IListarFaturamentoItensTabelaExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarItensTabelaExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FaturamentoItemTabelaDto> ItemTabelaDto)
        {
            return CreateExcelPackage(
                string.Format("ItensTabela_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ItensTabela"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Descricao")
                    );

                    AddObjects(
                        sheet, 2, ItemTabelaDto,
                        _ => _.Descricao
                            );

                });
        }
    }
}
