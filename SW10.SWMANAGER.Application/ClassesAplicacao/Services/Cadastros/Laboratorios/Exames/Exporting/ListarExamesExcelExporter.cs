using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Exporting;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Exames.Exporting
{
    public class ListarExamesExcelExporter : EpPlusExcelExporterBase, IListarExamesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarExamesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ExameDto> ExameDto)
        {
            return CreateExcelPackage(
                string.Format("Exames_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Exames"));
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
                        sheet, 2, ExameDto,
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
