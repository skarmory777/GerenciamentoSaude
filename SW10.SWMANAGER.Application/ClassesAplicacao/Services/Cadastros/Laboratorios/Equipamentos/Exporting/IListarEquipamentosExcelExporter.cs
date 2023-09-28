using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Exporting
{
    public interface IListarEquipamentosExcelExporter
    {
        FileDto ExportToFile(List<EquipamentoDto> EquipamentosDtos);
    }
}
