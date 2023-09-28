using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Exporting
{
    public class ListarEntradasExcelExporter : EpPlusExcelExporterBase, IListarEntradasExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarEntradasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<EntradaDto> paisDto)
        {
            return CreateExcelPackage(
                string.Format("Entradas_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Entradas"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EmpresaId"),
                        L("FornecedorId"),
                        L("TipoDocumentoId"),
                        L("CentroCustoId"),
                        L("NumeroDocumento"),
                        L("Data"),
                        L("AcrescimoDesconto"),
                        L("Frete"),
                        L("ValorDocumento")
                    );

                    AddObjects(
                        sheet, 2, paisDto,
                        _ => _.EmpresaId,
                        _ => _.FornecedorId,
                        _ => _.TipoDocumentoId,
                        _ => _.CentroCustoId,
                        _ => _.NumeroDocumento,
                        _ => _.Data,
                        _ => _.AcrescimoDesconto,
                        _ => _.Frete,
                        _ => _.ValorDocumento
                            );

                });
        }

    }
}
