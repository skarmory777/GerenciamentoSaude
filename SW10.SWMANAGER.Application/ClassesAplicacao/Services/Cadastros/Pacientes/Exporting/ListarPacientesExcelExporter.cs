using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Exporting
{
    public class ListarPacientesExcelExporter : EpPlusExcelExporterBase, IListarPacientesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarPacientesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<PacienteDto> pacientesDto)
        {
            return CreateExcelPackage(
                string.Format("Pacientes_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Pacientes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("NomeCompleto"),
                        L("Rg"),
                        L("Cpf"),
                        L("Nascimento"),
                        //L("Idade"),
                        L("Sexo"),
                        L("CorPele"),
                        L("Religiao"),
                        L("Prontuario")
                    );

                    AddObjects(
                        sheet, 2, pacientesDto,
                        _ => _.NomeCompleto,
                        _ => _.Rg,
                        _ => _.Cpf,
                        _ => _.Nascimento,
                        //_ => _.Idade,
                        _ => _.Sexo,
                        _ => _.CorPele,
                        _ => _.Religiao,
                        _ => _.Prontuario
                            );

                    //Formatting cells

                    var timeColumn1 = sheet.Column(4);

                    timeColumn1.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                });
        }
    }
}
