using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Exporting
{
    public class ListarCidadesExcelExporter : EpPlusExcelExporterBase, IListarCidadesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarCidadesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<CidadeDto> CidadeDto)
        {
            return CreateExcelPackage(
                string.Format("Cidades_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Cidades"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Nome"),
                        L("Estado"),
                        L("Capital")
                    );

                    AddObjects(
                        sheet, 2, CidadeDto,
                        _ => _.Nome,
                        _ => _.Estado.Nome + "(" + _.Estado.Uf + ")",
                        _ => _.Capital
                            );

                });
        }
    }
}
