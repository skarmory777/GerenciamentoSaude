using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Exporting
{
    public class ListarKitExamesExcelExporter : EpPlusExcelExporterBase, IListarKitExamesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarKitExamesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<KitExameDto> KitExameDto)
        {
            return CreateExcelPackage(
                string.Format("KitExames_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("KitExames"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Codigo"),
                        L("Descricao")
                    );

                    AddObjects(
                        sheet, 2, KitExameDto,
                        _ => _.Codigo,
                        _ => _.Descricao
                            );

                });
        }
    }
}
