using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos
{
    public interface IMetodoAppService : IApplicationService
    {
        Task<PagedResultDto<MetodoDto>> Listar(ListarMetodosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(MetodoDto input);

        Task Excluir(MetodoDto input);

        Task<MetodoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarMetodosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
