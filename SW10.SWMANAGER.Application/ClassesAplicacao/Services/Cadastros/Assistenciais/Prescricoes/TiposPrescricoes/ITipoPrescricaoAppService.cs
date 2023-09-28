using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes
{
    public interface ITipoPrescricaoAppService : IApplicationService
    {
        Task<PagedResultDto<TipoPrescricaoDto>> Listar(ListarInput input);

        Task<ListResultDto<TipoPrescricaoDto>> ListarTodos();

        Task<ListResultDto<TipoPrescricaoDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<TipoPrescricaoDto> CriarOuEditar(TipoPrescricaoDto input);

        Task Excluir(TipoPrescricaoDto input);

        Task<TipoPrescricaoDto> Obter(long id);
    }
}
