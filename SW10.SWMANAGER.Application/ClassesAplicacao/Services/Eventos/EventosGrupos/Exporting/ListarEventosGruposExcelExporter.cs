using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosGrupos.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosGrupos.Exporting
{
    public class ListarEventosGruposExcelExporter : EpPlusExcelExporterBase, IListarEventosGruposExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarEventosGruposExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<EventoGrupoDto> agendamentoConsultasDto)
        {
            return CreateExcelPackage(
                string.Format("Atendimentos_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Atendimentos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Descricao")

                    );

                    AddObjects(
                        sheet, 2, agendamentoConsultasDto,
                        _ => _.Id

                            );
                });
        }
    }
}
