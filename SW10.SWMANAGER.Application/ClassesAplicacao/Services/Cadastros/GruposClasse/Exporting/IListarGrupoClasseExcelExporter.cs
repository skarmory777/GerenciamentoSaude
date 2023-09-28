﻿using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Exporting
{
    public interface IListarGrupoClasseExcelExporter
    {
        FileDto ExportToFile(List<GrupoClasseDto> grupoClasseDtos);
    }
}