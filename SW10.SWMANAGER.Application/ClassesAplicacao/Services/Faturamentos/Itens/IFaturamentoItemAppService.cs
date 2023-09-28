using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens
{
    public interface IFaturamentoItemAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoItemDto>> Listar(ListarFaturamentoItensInput input);

        Task CriarOuEditar(FaturamentoItemDto input);

        Task Excluir(FaturamentoItemDto input);

        Task<FaturamentoItemDto> Obter(long id);

        Task<IEnumerable<FaturamentoItemDto>> ObterIds(List<long> ids);

        Task<IEnumerable<FaturamentoItemDto>> ObterPorCodigo(ObterPorCodigoDto input);

        Task<long> ObterTipoGrupoId(long? fatItemId);

        Task<FileDto> ListarParaExcel(ListarFaturamentoItensInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownContaMedica(DropdownInput dropdownInput);

        Task<ResultDropdownListDeluxe> ListarDropdownDeluxe(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownCodigo(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownExame(DropdownInput dropdownInput);

        Task<ListResultDto<FaturamentoItemDto>> ListarTodos();

        Task<IResultDropdownList<long>> ListarFatItemDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarExameLaboratorialDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarExameImagemDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarExameDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDiagnosticoImagemDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarFaturamentoItemPorGrupoSubGrupoDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownTodos(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownSemPacote(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownPacote(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarNaoLaudoNaoLaboratorioDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarCirurgiaAgendamentoDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarMateriaisOPMEDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarMaterialAgendamentoDropdown(DropdownInput dropdownInput);
    }
}

