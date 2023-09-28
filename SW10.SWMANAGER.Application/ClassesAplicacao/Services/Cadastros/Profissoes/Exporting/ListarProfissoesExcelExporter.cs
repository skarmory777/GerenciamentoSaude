using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Exporting
{
    public class ListarProfissoesExcelExporter : EpPlusExcelExporterBase, IListarProfissoesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarProfissoesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ProfissaoDto> profissoesDto)
        {
            return CreateExcelPackage(
                string.Format("Profissoes_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Profissoes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Descricao")
                    );

                    AddObjects(
                        sheet, 2, profissoesDto,
                        _ => _.Id,
                        _ => _.Descricao
                            );

                    //Formatting cells

                    //var timeColumn1 = sheet.Column(4);

                    //timeColumn1.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                });
        }
    }
}
