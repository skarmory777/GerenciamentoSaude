using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    public interface IFormulaFaturamentoAppService : IApplicationService
    {
        Task<PagedResultDto<FormulaFaturamentoDto>> Listar(ListarFormulaInput input);

        Task<PagedResultDto<FormulaFaturamentoDto>> ListarFatItem(ListarFormulaInput input);

        Task<PagedResultDto<FormulaFaturamentoDto>> ListarExameImagem(ListarFormulaInput input);

        Task<PagedResultDto<FormulaFaturamentoDto>> ListarExameLaboratorial(ListarFormulaInput input);

        Task<PagedResultDto<FormulaFaturamentoDto>> ListarFaturamentoJson(List<FormulaFaturamentoDto> list);

        Task<PagedResultDto<FormulaFaturamentoDto>> ListarExameLaboratorialJson(List<FormulaFaturamentoDto> list);

        Task<PagedResultDto<FormulaFaturamentoDto>> ListarExameImagemJson(List<FormulaFaturamentoDto> list);

        Task<ListResultDto<FormulaFaturamentoDto>> ListarTodos();

        Task<ListResultDto<FormulaFaturamentoDto>> ListarFaturamentoPorPrescricaoItem(long id);

        Task<ListResultDto<FormulaFaturamentoDto>> ListarExameLaboratorialPorPrescricaoItem(long id);

        Task<ListResultDto<FormulaFaturamentoDto>> ListarExameImagemPorPrescricaoItem(long id);

        Task<ListResultDto<FormulaFaturamentoDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarFormulaFaturamentoDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarFormulaExameLaboratorialDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarFormulaExameImagemDropdown(DropdownInput dropdownInput);

        Task<FormulaFaturamentoDto> CriarOuEditar(FormulaFaturamentoDto input);

        Task Excluir(FormulaFaturamentoDto input);

        Task<FormulaFaturamentoDto> Obter(long id);
    }
}
