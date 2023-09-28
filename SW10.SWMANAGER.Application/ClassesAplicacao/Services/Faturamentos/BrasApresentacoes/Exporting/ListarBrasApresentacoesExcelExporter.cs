using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Exporting
{
    public class ListarBrasApresentacoesExcelExporter : EpPlusExcelExporterBase, IListarBrasApresentacoesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarBrasApresentacoesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FaturamentoBrasApresentacaoDto> BrasApresentacaoDto)
        {
            return CreateExcelPackage(
                string.Format("BrasApresentacoes_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BrasApresentacoes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Descricao")
                    );

                    AddObjects(
                        sheet, 2, BrasApresentacaoDto,
                        _ => _.Descricao
                            );

                });
        }
    }
}
