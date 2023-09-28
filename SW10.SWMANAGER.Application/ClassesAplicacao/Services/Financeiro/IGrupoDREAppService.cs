using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface IGrupoDREAppService : IApplicationService
    {
        Task<ListResultDto<GrupoDREDto>> Listar(ListarGrupoDREInput input);
        Task<GrupoDREDto> Obter(long id);
        DefaultReturn<GrupoDREDto> CriarOuEditar(GrupoDREDto input);
        Task Excluir(GrupoDREDto input);
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
