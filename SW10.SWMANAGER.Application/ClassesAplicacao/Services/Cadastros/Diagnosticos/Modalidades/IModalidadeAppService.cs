using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades
{
    public interface IModalidadeAppService : IApplicationService
    {
        Task<ModalidadeDto> Obter(long id);
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
        Task CriarOuEditar(ModalidadeDto input);
        Task<PagedResultDto<ModalidadeDto>> Listar(ListarInput input);
        Task Excluir(ModalidadeDto input);
        Task<FileDto> ListarParaExcel(ListarInput input);
    }
}
