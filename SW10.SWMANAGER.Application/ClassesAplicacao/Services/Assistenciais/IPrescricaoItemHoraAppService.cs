using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface IPrescricaoItemHoraAppService : IApplicationService
    {
        Task<PagedResultDto<PrescricaoItemHoraDto>> Listar(ListarInput input);

        Task<ListResultDto<PrescricaoItemHoraDto>> ListarFiltro(string filtro);

        Task<ListResultDto<PrescricaoItemHoraDto>> ListarTodos();

        Task<ListResultDto<PrescricaoItemHoraDto>> ListarPorItem(long respostaId);

        Task CriarOuEditar(PrescricaoItemHoraDto input);

        Task Excluir(long id);

        Task<PrescricaoItemHoraDto> Obter(long id);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<List<PrescricaoItemHoraDto>> ObterListaPorItem(long respostaId);

    }
}
