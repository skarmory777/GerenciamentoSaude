using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    public interface ISisMoedaAppService : IApplicationService
    {
        Task<PagedResultDto<SisMoedaDto>> Listar(ListarSisMoedasInput input);

        Task CriarOuEditar(SisMoedaDto input);

        Task Excluir(SisMoedaDto input);

        Task<SisMoedaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarSisMoedasInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
