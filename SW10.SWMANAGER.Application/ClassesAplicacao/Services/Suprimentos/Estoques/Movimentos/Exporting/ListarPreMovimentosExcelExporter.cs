using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Movimentos.Exporting
{
    public class ListarPreMovimentosExcelExporter : EpPlusExcelExporterBase, IListarPreMovimentosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarPreMovimentosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<MovimentoIndexDto> preMovimentosDto)
        {
            return CreateExcelPackage(
                string.Format("Entrada_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Pacientes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Documento"),
                        L("Fornecedor"),
                        L("TotalDocumento")


                    );

                    AddObjects(
                        sheet, 2, preMovimentosDto,
                        _ => _.Documento,
                        _ => _.Fornecedor,
                        _ => _.Valor

                            );

                    //Formatting cells

                    var timeColumn1 = sheet.Column(4);

                    timeColumn1.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                });
        }
    }
}
