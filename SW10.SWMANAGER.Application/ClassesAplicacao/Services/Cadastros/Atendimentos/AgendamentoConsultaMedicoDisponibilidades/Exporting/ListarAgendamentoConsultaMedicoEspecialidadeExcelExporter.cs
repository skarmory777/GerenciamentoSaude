using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Exporting
{
    public class ListarAgendamentoConsultaMedicoDisponibilidadesExcelExporter : EpPlusExcelExporterBase, IListarAgendamentoConsultaMedicoDisponibilidadesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarAgendamentoConsultaMedicoDisponibilidadesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<AgendamentoConsultaMedicoDisponibilidadeDto> agendamentoConsultaMedicoDisponibilidadesDto)
        {
            return CreateExcelPackage(
                string.Format("AgendamentoConsultaMedicoDisponibilidades_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AgendamentoConsultaMedicoDisponibilidades"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Medico"),
                        L("Especialidade"),
                        L("DataInicio"),
                        L("DataFim"),
                        L("HoraInicio"),
                        L("HoraFim"),
                        L("Intervalo")


                    );

                    AddObjects(
                        sheet, 2, agendamentoConsultaMedicoDisponibilidadesDto,
                        _ => _.Id,
                        _ => _.Medico.SisPessoa?.NomeCompleto,
                        _ => _.MedicoEspecialidade.EspecialidadeId,
                        _ => _.DataInicio,
                        _ => _.DataFim,
                        _ => _.HoraInicio,
                        _ => _.HoraFim,
                        _ => _.Intervalo.Nome
                            );

                    //Formatting cells

                    var timeColumn4 = sheet.Column(4);
                    timeColumn4.Style.Numberformat.Format = "yyyy-mm-dd";

                    var timeColumn5 = sheet.Column(5);
                    timeColumn5.Style.Numberformat.Format = "yyyy-mm-dd";

                    var timeColumn6 = sheet.Column(6);
                    timeColumn6.Style.Numberformat.Format = "hh:mm:ss";

                    var timeColumn7 = sheet.Column(7);
                    timeColumn7.Style.Numberformat.Format = "hh:mm:ss";

                });
        }
    }
}
