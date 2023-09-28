using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Exporting
{
    public class ListarPaisesExcelExporter : EpPlusExcelExporterBase, IListarPaisesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarPaisesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<PaisDto> paisDto)
        {
            return CreateExcelPackage(
                string.Format("Paises_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Paises"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Nome"),
                        L("Sigla")
                    );

                    AddObjects(
                        sheet, 2, paisDto,
                        _ => _.Nome,
                        _ => _.Sigla
                            );

                });
        }
    }
}
