using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores
{
    public interface ISetorAppService : IApplicationService
    {
        Task<PagedResultDto<SetorDto>> Listar(ListarSetorsInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(SetorDto input);

        Task Excluir(SetorDto input);

        Task<SetorDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarSetorsInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
