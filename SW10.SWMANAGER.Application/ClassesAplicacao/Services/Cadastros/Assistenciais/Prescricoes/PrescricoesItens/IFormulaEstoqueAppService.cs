using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    public interface IFormulaEstoqueAppService : IApplicationService
    {
        Task<PagedResultDto<FormulaEstoqueDto>> Listar(ListarFormulaInput input);

        //Task<ListResultDto<FormulaEstoqueDto>> Listar(long prescricaoItemId);

        Task<PagedResultDto<FormulaEstoqueDto>> ListarJson(List<FormulaEstoqueDto> list);

        Task<ListResultDto<FormulaEstoqueDto>> ListarTodos();

        Task<ListResultDto<FormulaEstoqueDto>> ListarPorPrescricaoItem(long id);

        Task<ListResultDto<FormulaEstoqueDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<FormulaEstoqueDto> CriarOuEditar(FormulaEstoqueDto input);

        Task Excluir(FormulaEstoqueDto input);

        Task<FormulaEstoqueDto> Obter(long id);
    }
}
