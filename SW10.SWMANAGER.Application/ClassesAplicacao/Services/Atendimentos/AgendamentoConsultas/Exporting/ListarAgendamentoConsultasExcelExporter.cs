using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Exporting
{
    public class ListarAgendamentoConsultasExcelExporter : EpPlusExcelExporterBase, IListarAgendamentoConsultasExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarAgendamentoConsultasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<AgendamentoConsultaDto> agendamentoConsultasDto)
        {
            return CreateExcelPackage(
                string.Format("AgendamentoConsultas_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AgendamentoConsultas"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Medico"),
                        L("Especialidade"),
                        L("DiasSemana"),
                        L("Horario"),
                        L("Paciente")


                    );

                    AddObjects(
                        sheet, 2, agendamentoConsultasDto,
                        _ => _.Id,
                        _ => _.Medico.SisPessoa?.NomeCompleto,
                        _ => _.MedicoEspecialidade.EspecialidadeId,
                        _ => _.Paciente.NomeCompleto
                            );

                });
        }
    }
}
