using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus
{
    public interface IPrescricaoItemStatusAppService : IApplicationService
    {
        Task<PagedResultDto<PrescricaoItemStatusDto>> Listar(ListarInput input);

        Task<ListResultDto<PrescricaoItemStatusDto>> ListarTodos();

        Task<ListResultDto<PrescricaoItemStatusDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<PrescricaoItemStatusDto> CriarOuEditar(PrescricaoItemStatusDto input);

        Task Excluir(PrescricaoItemStatusDto input);

        Task<PrescricaoItemStatusDto> Obter(long id);
    }
}
