﻿using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Exporting
{
    public interface IListarFaturamentoGlobaisExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoGlobalDto> globaisDtos);
    }
}
