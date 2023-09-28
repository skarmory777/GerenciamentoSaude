using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Exporting
{
    public class ListarModalidadeExcelExporter : EpPlusExcelExporterBase, IListarModalidadeExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarModalidadeExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ModalidadeDto> modalidadesDto)
        {
            return CreateExcelPackage(
               string.Format("Estados_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
               excelPackage =>
               {
                   var sheet = excelPackage.Workbook.Worksheets.Add(L("Estados"));
                   sheet.OutLineApplyStyle = true;

                   AddHeader(
                       sheet,
                       L("Id"),
                       L("Descricao")
                   );

                   AddObjects(
                       sheet, 2, modalidadesDto,
                       _ => _.Id,
                       _ => _.Descricao
                           );

               });
        }
    }
}