using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Exporting
{
    public class ListarCentrosCustosExcelExporter : EpPlusExcelExporterBase, IListarCentrosCustosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarCentrosCustosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<CentroCustoDto> centrosCustosDto)
        {
            return CreateExcelPackage(
                string.Format("CentrosCustos{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("CentrosCustos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Descricao"),
                        L("GrupoCentroCusto"),
                        L("CodigoCentroCusto"),
                         L("UnidadeOrganizacional"),
                        L("IsReceberLancamento"),
                        L("IsAtivo")
                    );

                    AddObjects(
                        sheet, 2, centrosCustosDto,
                        _ => _.Descricao,
                        _ => _.GrupoCentroCusto.Descricao,
                        _ => _.CodigoCentroCusto,
                        _ => _.UnidadeOrganizacional.Descricao,
                        _ => _.IsReceberLancamento,
                        _ => _.IsAtivo
                            );

                    //Formatting cells

                    //var timeColumn1 = sheet.Column(4);

                    //timeColumn1.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                });
        }
    }
}
