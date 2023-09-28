using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Exporting
{
    public class ListarLaboratorioUnidadesExcelExporter : EpPlusExcelExporterBase, IListarLaboratorioUnidadesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarLaboratorioUnidadesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<LaboratorioUnidadeDto> LaboratorioUnidadeDto)
        {
            return CreateExcelPackage(
                string.Format("LaboratorioUnidades_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LaboratorioUnidades"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Codigo"),
                        L("Descricao")
                    );

                    AddObjects(
                        sheet, 2, LaboratorioUnidadeDto,
                        _ => _.Codigo,
                        _ => _.Descricao
                            );

                });
        }
    }
}
