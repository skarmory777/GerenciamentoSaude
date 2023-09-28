using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Exporting
{
    public class ListarProdutoEmpresaExcelExporter : EpPlusExcelExporterBase, IListarProdutoEmpresaExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarProdutoEmpresaExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ProdutoEmpresaDto> produtoEmpresaDtos)
        {
            return CreateExcelPackage(
                string.Format("ProdutosEmpresa_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProdutosEmpresa"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        //L("Nome"),
                        //L("Descricao"),
                        //L("Fator"),
                        L("CreatorUserId"),
                        L("CreationTime"),
                        L("LastModifierUserId"),
                        L("LastModificationTime"),
                        L("DeleterUserId"),
                        L("DeletionTime")
                    //L("Browser"),
                    //L("ErrorState")
                    );

                    AddObjects(
                        sheet, 2, produtoEmpresaDtos,
                        ////_ => _timeZoneConverter.Convert(_.ExecutionTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        //_ => _.ProdutoNome,
                        //_ => _.Descricao,
                        //_ => _.Fator,
                        _ => _.CreatorUserId,
                        _ => _.CreationTime,
                        _ => _.LastModifierUserId,
                        _ => _.LastModificationTime,
                        _ => _.DeleterUserId,
                        _ => _.DeletionTime
                        //_ => _.BrowserInfo,
                        //_ => _.Exception.IsNullOrEmpty() ? L("Success") : _.Exception
                            );

                    //Formatting cells

                    /*var timeColumn = */
                    var timeColumn1 = sheet.Column(4);
                    var timeColumn2 = sheet.Column(6);
                    var timeColumn3 = sheet.Column(8);

                    timeColumn1.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    timeColumn2.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    timeColumn3.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    //for (var i = 1; i <= 10; i++)
                    //{
                    //    if (i.IsIn(5, 10)) //Don't AutoFit Parameters and Exception
                    //    {
                    //        continue;
                    //    }

                    //    sheet.Column(i).AutoFit();
                    //}
                });
        }
    }
}