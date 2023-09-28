using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces
{
    public interface IPainelAppService : IApplicationService
    {
        Task Excluir(long id);
        Task<PainelDto> Obter(long id);
        Task<PagedResultDto<PainelDto>> Listar(ListarPainelSenhaInput input);
        DefaultReturn<PainelDto> CriarOuEditar(PainelDto input);
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
