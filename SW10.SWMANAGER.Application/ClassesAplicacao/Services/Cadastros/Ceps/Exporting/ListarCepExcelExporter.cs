using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Exporting
{
    public class ListarCepExcelExporter : EpPlusExcelExporterBase, IListarCepExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarCepExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<CepDto> CepDtos)
        {
            return CreateExcelPackage(
                  string.Format("Ceps_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                  excelPackage =>
                  {
                      var sheet = excelPackage.Workbook.Worksheets.Add(L("CEPs"));
                      sheet.OutLineApplyStyle = true;

                      AddHeader(
                          sheet,
                          L("Cep"),
                          L("Logradouro"),
                          L("Bairro"),
                          L("Estado")
                      );

                      AddObjects(
                          sheet, 2, CepDtos,
                          _ => _.CEP,
                          _ => _.Logradouro,
                          _ => _.Bairro,
                          _ => _.Estado.Nome
                        );
                  });
        }
    }
}
