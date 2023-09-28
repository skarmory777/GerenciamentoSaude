using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados
{
    public interface IItemResultadoAppService : IApplicationService
    {
        Task<PagedResultDto<ItemResultadoDto>> Listar(ListarItemResultadosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(ItemResultadoDto input);

        Task Excluir(ItemResultadoDto input);

        Task<ItemResultadoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarItemResultadosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
