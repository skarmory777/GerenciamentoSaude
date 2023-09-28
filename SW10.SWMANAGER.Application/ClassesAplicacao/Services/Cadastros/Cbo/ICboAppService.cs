using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cbos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cbos
{
    public interface ICboAppService : IApplicationService
    {
        Task<PagedResultDto<CboDto>> Listar(ListarCbosInput input);

        Task CriarOuEditar(CboDto input);

        Task Excluir(CboDto input);

        Task<CboDto> Obter(long id);

        //     Task<FileDto> ListarParaExcel(ListarCbosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
