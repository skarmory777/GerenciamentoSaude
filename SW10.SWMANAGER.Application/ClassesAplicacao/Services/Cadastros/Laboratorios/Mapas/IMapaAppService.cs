using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas
{
    public interface IMapaAppService : IApplicationService
    {
        Task<PagedResultDto<MapaDto>> Listar(ListarMapasInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(MapaDto input);

        Task Excluir(MapaDto input);

        Task<MapaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarMapasInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
