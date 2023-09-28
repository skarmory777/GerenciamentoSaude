using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus
{
    public interface IPrescricaoStatusAppService : IApplicationService
    {
        Task<PagedResultDto<PrescricaoStatusDto>> Listar(ListarInput input);

        Task<ListResultDto<PrescricaoStatusDto>> ListarTodos();

        Task<ListResultDto<PrescricaoStatusDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<PrescricaoStatusDto> CriarOuEditar(PrescricaoStatusDto input);

        Task Excluir(PrescricaoStatusDto input);

        Task<PrescricaoStatusDto> Obter(long id);
    }
}
