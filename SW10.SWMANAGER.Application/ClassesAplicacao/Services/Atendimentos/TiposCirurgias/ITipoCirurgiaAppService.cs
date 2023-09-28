using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.TiposCirurgias
{
    public interface ITipoCirurgiaAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        Task<TipoCirurgiaDto> Obter(long id);
    }
}
