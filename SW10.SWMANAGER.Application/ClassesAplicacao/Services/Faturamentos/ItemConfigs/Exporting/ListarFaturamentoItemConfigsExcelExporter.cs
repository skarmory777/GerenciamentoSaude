﻿using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Exporting
{
    public class ListarItemConfigsExcelExporter : EpPlusExcelExporterBase, IListarItemConfigsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarItemConfigsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FaturamentoItemConfigDto> ItemConfigsDto)
        {
            return CreateExcelPackage(
                string.Format("ItemConfigs_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ItemConfigs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ConvenioId")
                    );

                    AddObjects(
                        sheet, 2, ItemConfigsDto,
                        _ => _.ConvenioId
                            );

                });
        }
    }
}
