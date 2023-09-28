using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles
{
    public interface ITipoControleAppService : IApplicationService
    {
        Task<PagedResultDto<TipoControleDto>> Listar(ListarInput input);

        Task<ListResultDto<TipoControleDto>> ListarTodos();

        Task<ListResultDto<TipoControleDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<TipoControleDto> CriarOuEditar(TipoControleDto input);

        Task Excluir(TipoControleDto input);

        Task<TipoControleDto> Obter(long id);
    }
}
