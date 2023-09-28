using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Exporting
{
    public class ListarTiposSanguineosExcelExporter : EpPlusExcelExporterBase, IListarTiposSanguineosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarTiposSanguineosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<TipoSanguineoDto> tipoSanguineoDto)
        {
            return CreateExcelPackage(
                string.Format("TiposSanguineos_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TiposSanguineos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Codigo")
                    );

                    AddObjects(
                        sheet, 2, tipoSanguineoDto,
                        _ => _.Codigo
                            );

                });
        }
    }
}
