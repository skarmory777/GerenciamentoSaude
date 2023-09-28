using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Exporting
{
    public class ListarGruposCentrosCustosExcelExporter : EpPlusExcelExporterBase, IListarGruposCentrosCustosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarGruposCentrosCustosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GrupoCentroCustoDto> gruposCentrosCustosDto)
        {
            return CreateExcelPackage(
                string.Format("GruposCentrosCustos_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GruposCentrosCustos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Descricao"),
                        L("TipoGrupoCentroCustos")
                    );

                    AddObjects(
                        sheet, 2, gruposCentrosCustosDto,
                        _ => _.Descricao
                        //,
                        //_ => _.TipoGrupoCentroCustos.Descricao
                            );

                });
        }
    }
}
