using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados
{
    public interface IEstadoAppService : IApplicationService
    {
        Task<PagedResultDto<EstadoDto>> Listar(ListarEstadosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long? paisId);

        Task CriarOuEditar(EstadoDto input);

        Task Excluir(EstadoDto input);

        Task<EstadoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarEstadosInput input);

        Task<ListResultDto<EstadoDto>> ListarPorPais(long? id);

        Task<EstadoDto> Obter(string uf);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
