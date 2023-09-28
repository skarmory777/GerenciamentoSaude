using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos
{
    public interface ILaudoGrupoAppService : IApplicationService
    {
        Task<PagedResultDto<LaudoGrupoDto>> Listar(ListarInput input);
        Task<ListResultDto<LaudoGrupoDto>> ListarTodos();

        Task CriarOuEditar(LaudoGrupoDto input);

        Task Excluir(LaudoGrupoDto input);

        Task<LaudoGrupoDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
