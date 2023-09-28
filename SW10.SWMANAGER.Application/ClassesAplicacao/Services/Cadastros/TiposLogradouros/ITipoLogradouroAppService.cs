using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros
{
    public interface ITipoLogradouroAppService : IApplicationService
    {
        Task<PagedResultDto<TipoLogradouroDto>> Listar(ListarTiposLogradouroInput input);

        Task<ListResultDto<TipoLogradouroDto>> ListarTodos();

        Task<CriarOuEditarTipoLogradouroDto> CriarOuEditar(CriarOuEditarTipoLogradouroDto input);

        Task Excluir(CriarOuEditarTipoLogradouroDto input);

        Task<CriarOuEditarTipoLogradouroDto> Obter(long id);

        Task<CriarOuEditarTipoLogradouroDto> Obter(string logradouro);

        Task<FileDto> ListarParaExcel(ListarTiposLogradouroInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

    }
}
