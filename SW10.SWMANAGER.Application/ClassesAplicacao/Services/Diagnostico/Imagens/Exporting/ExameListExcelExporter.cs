using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Exporting
{
    public class ExameListExcelExporter : EpPlusExcelExporterBase, IExameListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExameListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ExameListDto> exameListDtos)
        {
            return CreateExcelPackage(
                "ExameList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Exames"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Codigo"),
                        L("CodigoAtendimento"),
                        L("Paciente"),
                        L("Médico"),
                        L("TemContraste"),
                        L("Quantidade"),
                        L("Situacao")
                        );

                    AddObjects(
                        sheet, 2, exameListDtos,
                        _ => _.Codigo,
                        _ => _.Atendimento.Codigo,
                        _ => _.Atendimento.Paciente.NomeCompleto,
                        _ => _.Atendimento.Medico.NomeCompleto,
                        _ => _.IsContraste,
                        _ => _.QtdeConstraste,
                         _ => _.LaudoMovimentoStatus.Descricao
                        );

                    //Formatting cells

                    //var lastLoginTimeColumn = sheet.Column(8);
                    //lastLoginTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";

                    //var creationTimeColumn = sheet.Column(10);
                    //creationTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";

                    for (var i = 1; i <= 7; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
