using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Exporting
{
    public class ListarMaterialsExcelExporter : EpPlusExcelExporterBase, IListarMaterialsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarMaterialsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<MaterialDto> MaterialDto)
        {
            return CreateExcelPackage(
                string.Format("Materials_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Materials"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Codigo"),
                        L("Descricao")
                    //L("TipoLayout"),
                    //L("DiretorioOrdem"),
                    //L("DiretorioResultado"),
                    //L("Informacao")
                    );

                    AddObjects(
                        sheet, 2, MaterialDto,
                        _ => _.Codigo,
                        _ => _.Descricao
                        //_ => _.TipoLayout,
                        //_ => _.DiretorioOrdem,
                        //_ => _.DiretorioResultado,
                        //_ => _.Informacao == null ? "" : _.Informacao.Descricao
                            );

                });
        }
    }
}
