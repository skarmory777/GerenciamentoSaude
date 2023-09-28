using Abp.Runtime.Session;
using Abp.Timing.Timezone;

using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo.Exporting
{
    public class ListarFaturamentoTiposGrupoExcelExporter : EpPlusExcelExporterBase, IListarFaturamentoTiposGrupoExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarFaturamentoTiposGrupoExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FaturamentoItemDto> ItemDto)
        {
            return CreateExcelPackage(
                string.Format("FaturamentoTiposGrupo_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("FaturamentoTiposGrupo"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Descricao")
                    );

                    AddObjects(
                        sheet, 2, ItemDto,
                        _ => _.Descricao
                            );

                });
        }
    }
}
