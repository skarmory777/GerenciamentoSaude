using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.SalasCirurgicas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.SalasCirurgicas.Servicos
{
    public interface ISalaCirurgicaAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        Task<List<SalaCirurgicaDto>> Listar();
        Task<SalaCirurgicaDto> Obter(long id);
    }
}
