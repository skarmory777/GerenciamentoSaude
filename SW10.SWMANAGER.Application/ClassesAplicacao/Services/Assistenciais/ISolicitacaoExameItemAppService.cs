using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface ISolicitacaoExameItemAppService : IApplicationService
    {
        Task<PagedResultDto<SolicitacaoExameItemList>> Listar(ListarSolicitacaoExameItensInput input);

        Task<ListResultDto<SolicitacaoExameItemDto>> ListarFiltro(string filtro);

        Task<ListResultDto<SolicitacaoExameItemDto>> ListarTodos();

        Task<PagedResultDto<SolicitacaoExameItemList>> ListarPorSolicitacao(ListarSolicitacaoExameItensInput input);

        Task<PagedResultDto<SolicitacaoExameItemList>> ListarAtendimento(ListarSolicitacaoExameItensInput input);

        Task<SolicitacaoExameItemDto> CriarOuEditar(SolicitacaoExameItemDto input);

        Task Excluir(long id);

        Task<SolicitacaoExameItemDto> Obter(long id);

        Task<SolicitacaoExameItemDto> ObterParaEdicao(long id);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownNaoRegistradoPorAtendimento(DropdownInput dropdownInput);

        Task<PagedResultDto<RegistroExameIndex>> ListarExamesImagensNaoRegistrados(ListarExameSolicitadosInput input);

        Task<List<SolicitacaoExameItemDto>> ObterPorLista(List<long> ids);

        Task<PagedResultDto<RegistroExameIndex>> ListarExamesLaboratoriaisNaoColetados(ListarExameSolicitadosInput input);

        Task<IResultDropdownList<long>> ListarDropdownExameLaboratorioNaoRegistradoPorAtendimento(DropdownInput dropdownInput);

    }
}
