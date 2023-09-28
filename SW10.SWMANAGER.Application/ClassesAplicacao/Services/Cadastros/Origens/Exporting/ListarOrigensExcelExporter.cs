using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Exporting
{
    public class ListarOrigensExcelExporter : EpPlusExcelExporterBase, IListarOrigensExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarOrigensExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<OrigemDto> origensDto)
        {
            return CreateExcelPackage(
                string.Format("Origens_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Origens"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Descricao"),
                        L("UnidadeOrganizacional"),
                        L("IsAtivo")
                    );

                    AddObjects(
                        sheet, 2, origensDto,
                        _ => _.Descricao,
                        _ => _.UnidadeOrganizacional.Descricao,
                        _ => _.IsAtivo
                            );

                    //Formatting cells

                    //var timeColumn1 = sheet.Column(4);

                    //timeColumn1.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                });
        }
    }
}
