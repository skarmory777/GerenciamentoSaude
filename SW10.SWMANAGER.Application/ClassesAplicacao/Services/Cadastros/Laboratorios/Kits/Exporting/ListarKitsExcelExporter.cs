using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Kits.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Kits.Exporting;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Kits.Exporting
{
    public class ListarKitsExcelExporter : EpPlusExcelExporterBase, IListarKitsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarKitsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<KitDto> KitDto)
        {
            return CreateExcelPackage(
                string.Format("Kits_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Kits"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Codigo"),
                        L("Descricao"),
                        L("TipoLayout"),
                        L("DiretorioOrdem"),
                        L("DiretorioResultado"),
                        L("Informacao")
                    );

                    AddObjects(
                        sheet, 2, KitDto,
                        _ => _.Codigo,
                        _ => _.Descricao,
                        _ => _.TipoLayout,
                        _ => _.DiretorioOrdem,
                        _ => _.DiretorioResultado
                        //_ => _.Informacao == null ? "" : _.Informacao.Descricao
                            );

                });
        }
    }
}
