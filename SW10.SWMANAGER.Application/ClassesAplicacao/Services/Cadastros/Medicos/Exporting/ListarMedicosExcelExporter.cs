using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Exporting
{
    public class ListarMedicosExcelExporter : EpPlusExcelExporterBase, IListarMedicosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarMedicosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ListarMedicoIndex> medicosDto)
        {
            return CreateExcelPackage(
                string.Format("Medicos_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Medicos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("NomeFantasia"),
                        L("NumeroConselho")
                    );

                    AddObjects(
                        sheet, 2, medicosDto,
                        _ => _.Id,
                        _ => _.NomeCompleto,
                        _ => _.NumeroConselho
                            );
                });
        }
    }
}
