﻿using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Exporting
{
    public interface IListarTiposAtestadosExcelExporter
    {
        FileDto ExportToFile(List<TipoAtestadoDto> list);
    }
}
