using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Exporting
{
    public class ListarTipoLogradouroExcelExporter : EpPlusExcelExporterBase, IListarTipoLogradouroExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarTipoLogradouroExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<TipoLogradouroDto> tipoLogradouroDtos)
        {
            return CreateExcelPackage(
                  string.Format("TiposLogradouros_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                  excelPackage =>
                  {
                      var sheet = excelPackage.Workbook.Worksheets.Add(L("TiposLogradouros"));
                      sheet.OutLineApplyStyle = true;

                      AddHeader(
                          sheet,
                          L("Abreviacao"),
                          L("Descricao")
                      );

                      AddObjects(
                          sheet, 2, tipoLogradouroDtos,
                          _ => _.Abreviacao,
                          _ => _.Descricao
                        );
                  });
        }
    }
}