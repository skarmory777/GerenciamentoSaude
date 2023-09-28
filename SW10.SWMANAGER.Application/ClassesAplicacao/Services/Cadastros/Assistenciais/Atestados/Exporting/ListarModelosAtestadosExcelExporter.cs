﻿using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.DataExporting.Excel.EpPlus;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Exporting
{
    public class ListarModelosAtestadosExcelExporter : EpPlusExcelExporterBase, IListarModelosAtestadosExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ListarModelosAtestadosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ModeloAtestadoDto> modeloAtestadoDto)
        {
            return CreateExcelPackage(
                string.Format("Estados_{0:yyyyMMdd_hhmmss}.xlsx", DateTime.Now),
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Estados"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Id"),
                        L("Conteudo")
                    );

                    AddObjects(
                        sheet, 2, modeloAtestadoDto,
                        _ => _.Id,
                        _ => _.Conteudo
                            );

                });
        }
    }
}
