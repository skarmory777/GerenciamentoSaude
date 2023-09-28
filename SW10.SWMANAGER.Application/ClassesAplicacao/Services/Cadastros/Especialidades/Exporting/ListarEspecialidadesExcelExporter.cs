using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Exporting
{
    public class ListarEspecialidadesExcelExporter : EpPlusExcelExporterBase, IListarEspecialidadesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarEspecialidadesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<EspecialidadeDto> especialidadesDto)
        {
            return CreateExcelPackage(
                string.Format("Especialidades_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Especialidades"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Nome")
                    );

                    AddObjects(
                        sheet, 2, especialidadesDto,
                        _ => _.Id,
                        _ => _.Nome
                            );

                    //Formatting cells

                    //var timeColumn1 = sheet.Column(4);

                    //timeColumn1.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                });
        }
    }
}
