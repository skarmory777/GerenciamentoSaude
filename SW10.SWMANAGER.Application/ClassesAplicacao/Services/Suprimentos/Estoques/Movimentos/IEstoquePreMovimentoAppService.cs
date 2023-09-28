using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;
using System.Web.Mvc;
using static SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.EstoquePreMovimentoAppService;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEstoquePreMovimentoAppService : IApplicationService
    {
        Task<PagedResultDto<MovimentoIndexDto>> Listar(ListarEstoquePreMovimentoInput input);

        Task<ListResultDto<EstoquePreMovimentoDto>> ListarTodos();

        Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditar(EstoquePreMovimentoDto input);

        Task Excluir(EstoquePreMovimentoDto input);

        Task<EstoquePreMovimentoDto> Obter(long id);

        Task<EstoquePreMovimentoDto> ObterParaConfirmarSolicitacao(long id);

        Task<TipoMovimentoDto> ObterTipoMovimentoEntrada(long id);

        Task<PagedResultDto<MovimentoItemDto>> ListarItens(ListarEstoquePreMovimentoInput input);

        EstoquePreMovimentoDto CriarGetIdEntrada(EstoquePreMovimentoDto input);

        Task<FileDto> ListarParaExcel(ListarEstoquePreMovimentoInput input);

        Task<bool> PermiteConfirmarEntrada(EstoquePreMovimentoDto preMovimento);

        EstoquePreMovimentoDto CriarGetIdSaida(EstoquePreMovimentoDto input);

        Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditarSaida(EstoquePreMovimentoDto input);

        Task<PagedResultDto<MovimentoItemDto>> ListarItensSaida(ListarEstoquePreMovimentoInput input);

        Task<DefaultReturn<EstoqueTransferenciaProdutoDto>> TransferirProduto(EstoqueTransferenciaProdutoDto transferenciaProdutoDto);

        Task<PagedResultDto<EstoqueTransferenciaProdutoDto>> ListarTransferencia(ListarEstoquePreMovimentoInput input);

        Task<EstoqueTransferenciaProdutoDto> ObterTransferencia(long id);

        Task<PagedResultDto<MovimentoItemDto>> ListarItensTranferencia(ListarEstoquePreMovimentoInput input);

        Task ExcluirTransferencia(long transferenciaId);

        Task<PagedResultDto<MovimentoIndexDto>> ListarMovimentosPendente(ListarEstoquePreMovimentoInput input);

        DefaultReturn<EstoquePreMovimentoDto> CriarGetIdDevolucao(EstoquePreMovimentoDto input);
        Task<PagedResultDto<MovimentoIndexDto>> ListarDevolucoes(ListarEstoquePreMovimentoInput input);
        DefaultReturn<EstoquePreMovimentoDto> CriarOuEditarDevolucoes(EstoquePreMovimentoDto input);

        Task<PagedResultDto<MovimentoIndexDto>> ListarSolicitacoes(ListarEstoquePreMovimentoInput input);

        DefaultReturn<EstoquePreMovimentoDto> CriarOuEditarSolicitacao(EstoquePreMovimentoDto input);

        [HttpPost]
        Task<PagedResultDto<EstoquePreMovimentoItemSolicitacaoDto>> ListarItensJson(ListarItensJsonInput input);

        Task<PagedResultDto<MovimentoIndexDto>> ListarSolicitacaoesPendente(ListarEstoquePreMovimentoInput input);

        Task<DefaultReturn<EstoquePreMovimentoDto>> AtenderSolicitacao(EstoquePreMovimentoDto preMovimento);

        RelatorioEntradaModelDto ObterDadosRelatorioEntrada(long preMovimentoId);

        Task<EstoqueTransferenciaProdutoDto> ObterTransferenciaPorEntradaId(long id);

        RelatorioSolicitacaoSaidaModelDto ObterDadosRelatorioSolicitacao(long solicitacaoId);

        Task<DefaultReturn<DocumentoDto>> ExcluirSolicitacoesPrescritasNaoAtendidas(long prescricaoMedicaId);

        Task<DefaultReturn<DocumentoDto>> ExcluirSolicitacoesPrescritasNaoAtendidasPorItemResposta(long prescricaoItemRespostaId);

        Task<DefaultReturn<EstoquePreMovimento>> ReAtivarSolicitacoDePrescricaoMedica(long prescricaoMedicaId);

        Task<DefaultReturn<EstoquePreMovimento>> ReAtivarSolicitacoDePrescricaoItemResposta(long prescricaoItemRespostaId);

        Task<PagedResultDto<MovimentoIndexDto>> ListarSolicitacoesEmprestimos(ListarEstoquePreMovimentoInput input);

        Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditarEmprestimoEntrada(EstoquePreMovimentoDto input);

        Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditarEmprestimoSaida(EstoquePreMovimentoDto input);

        bool ChaveNFeUtilizada(string chave, long? movimentoId);

        Task<IResultDropdownList<long>> ListarDropdownPreMovimentoEstado(DropdownInput dropdownInput);
        byte[] RetornaArquivoSolicitacaoBaixa(long preMovimentoId);
        byte[] RetornaArquivoSolicitacao(long preMovimentoId);

        Task<IEnumerable<MovimentoIndexDto>> BuscarPorPrescricaoMedica(string prescricaoMedicaId);

        Task<MovimentoIndexDto> BuscarPorSolicitacao(string solicitacaoId);

        Task Excluir(long preMovimentoId);
    }
}
