﻿using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Exporting
{
    public interface IListarEspecialidadesExcelExporter
    {
        FileDto ExportToFile(List<EspecialidadeDto> profissoesDtos);
    }
}
