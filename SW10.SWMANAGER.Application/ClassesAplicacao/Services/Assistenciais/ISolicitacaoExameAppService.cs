using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface ISolicitacaoExameAppService : IApplicationService
    {
        Task<PagedResultDto<SolicitacaoExameIndex>> Listar(ListarSolicitacoesExamesInput input);

        Task<ListResultDto<SolicitacaoExameDto>> ListarFiltro(string filtro);

        Task<ListResultDto<SolicitacaoExameDto>> ListarTodos();

        Task<DefaultReturn<SolicitacaoExameDto>> CriarOuEditar(SolicitacaoExameDto input);

        Task RequisitarSolicitacao(SolicitacaoExameInputDto input);

        Task Excluir(long id);

        Task<SolicitacaoExameDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<SolicitacaoExameDto> ObterParaImpressao(long id);

        bool ValidarSolicitacaoExame(long atendimentoId);


        byte[] RetornaArquivoSolicitacaoExame(long solicitacaoExameId);

        Task<LaboratorioPainelDetalhamentoCounters> RetornaContadores(long? labResultadoId);

        Task<PagedResultDto<ExamesDetalhamentoViewModel>> RetornaExamesPorSolicitacaoId(ExamesDetalhamentoInput dto);
    }
}
