using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using System;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public interface IProdutoAppService : IApplicationService
    {
        #region ↓ Metodos

        #region → Basico - CRUD

        /// <summary>
        /// Cria ou Edita um produto, considerando se o valor do atributo ID possui valor ou nao
        /// </summary>
        /// <param name="input">Dto de Produto</param>
        /// <returns>Sem retorno</returns>
        Task CriarOuEditar(ProdutoDto input);

        /// <summary>
        /// Exclui um produto
        /// </summary>
        /// <param name="input">Dto de Produto</param>
        /// <returns>Sem retorno</returns>
        Task Excluir(ProdutoDto input);

        /// <summary>
        /// Retorna um Dto de Produto
        /// </summary>
        /// <param name="id">Id do produto desejado</param>
        /// <returns>Dto de Produto</returns>
        Task<ProdutoDto> Obter(long id);

        #endregion Basico - CRUD

        #region → Basico - Listar

        /// <summary>
        /// Retorna uma lista de Produtos preparada para popular um JTable
        /// </summary>
        /// <param name="input">ListarProdutosInput</param>
        /// <returns>Lista paginada de Dtos de Produto</returns>
        Task<PagedResultDto<ProdutoDto>> Listar(ListarProdutosInput input);

        /// <summary>
        /// Retorna uma lista de produto
        /// </summary>
        /// <returns>Lista de Dtos de produto</returns>
        Task<ListResultDto<ProdutoDto>> ListarTodos();

        #endregion Basico - Listar

        #region → Obter
        /// <summary>
        /// Retorna o próximo codigo disponivel. Ps: Não é o campo Id
        /// </summary>
        string ObterProximoNumero(ProdutoDto input);

        /// <summary>
        /// Cria um novo produto e retorna um ProdutoDto com seu Id
        /// </summary>
        ProdutoDto CriarGetId(ProdutoDto input);

        Task<ListResultDto<UnidadeDto>> ObterUnidadePorProduto(long id);

        Task<ListResultDto<UnidadeDto>> ObterUnidadePorProduto(long id, bool listarRefGer = true);

        /// <summary>
        /// Retorna a Unidade Referencia ou Gerencial do produto
        /// Referencia: idTipoUnidade = 1 | Gerencial: idTipoUnidade = 2
        /// </summary>
        Task<UnidadeDto> ObterUnidadePorTipo(long idProduto, long idTipoUnidade);

        /// <summary>
        /// Retorna a Unidade Refekrencia do produto - UnidadeTipoId = 1
        /// </summary>
        Task<UnidadeDto> ObterUnidadeReferencial(long idProduto);

        /// <summary>
        /// Retorna a Unidade Gerencial do produto (UnidadeTipoId = 2)
        /// </summary>
        Task<UnidadeDto> ObterUnidadeGerencial(long idProduto);

        #endregion

        #region → Gets

        /// <summary>
        /// Indica se há movimentação de estoque para o produto
        /// </summary>
        /// <param name="id">Id do produto desejado</param>
        /// <returns>bool</returns>
        Task<bool> ExisteMovimentacaoDeEstoque(long id);

        /// <summary>
        /// Indica se um produto possui Requisição de Compra
        /// </summary>
        /// <param name="id">Id do produto desejado</param>
        /// <returns>bool</returns>
        Task<bool> ExisteRequisicaoDeCompraPendenteParaOProduto(long id);

        #endregion

        #region → Listar

        /// <summary>
        /// Retorna uma lista de produto ativos e não bloqueados para compra
        /// </summary>
        /// <returns>Lista de Dtos de produto</returns>
        Task<ListResultDto<ProdutoDto>> ListarTodosParaMovimento();

        /// <summary>
        /// 
        /// </summary>
        Task<ListResultDto<ProdutoDto>> ListarProdutosExcetoId(long ProdutoExcetoid);

        /// <summary>
        /// Retorna Lista com todos os produtos definidos como Principal(Mestre)
        /// </summary>
        /// <returns>ListResultDto<GenericoIdNome> de Produto</returns>
        Task<ListResultDto<GenericoIdNome>> ListarProdutosMestre();

        /// <summary>
        /// Retorna uma lista com Nome(descricao) e Id de produtos.
        /// </summary>
        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        /// <summary>
        /// ListResultDto de DCB
        /// </summary>
        //Task<ListResultDto<GenericoIdNome>> ListarDCBs();
                
        Task<ListResultDto<ProdutoDto>> ListarProdutosMestresExcetoId(long ProdutoExcetoid);

        Task<PagedResultDto<ProdutoUnidadeDto>> ListarProdutoUnidade(long produtoid);

        Task<PagedResultDto<ProdutoUnidadeDto>> ListarProdutosUnidadesPorProduto(ListarInput input);

        Task<PagedResultDto<ProdutoEmpresaDto>> ListarProdutosEmpresasPorProduto(ListarInput input);
                
        Task<PagedResultDto<ProdutoEstoqueDto>> ListarProdutosEstoquesPorProduto(ListarInput input);

        Task<PagedResultDto<ProdutoSaldoMinDto>> ListarProdutoSaldo(ListarInput input);

        Task<PagedResultDto<ProdutoSaldoDto>> ListarProdutoSaldoDetalhes(ListarSaldoInput input);

        Task<PagedResultDto<ProdutoSaldoMinDto>> ListarProdutoSaldoFilhos(ListarInput input);

        Task<PagedResultDto<ProdutoDto>> ListarProdutoMesmoPrincipal(ListarInput input);

        Task<PagedResultDto<ProdutoFornecedorDto>> ListarProdutoFornecedores(ListarInput input);

        Task<PagedResultDto<ProdutoSaldoMinDto>> ListarProdutoMesmoPrincipalComSaldo(ListarInput input);

        Task<ListResultDto<VWRptSaldoProdutoDto>> ListarProdutoSaldoReport(long estoqueId, long grupoId);

        Task<ListResultDto<VWRptEstoqueMovimentoResumidoDto>> ListarEstoqueMovimentoResumidoReport(DateTime startDate, DateTime endDate, long estoqueId, long grupoId, string produto, int tipoRel = 1);

        Task<ListResultDto<VWRptEstoqueMovimentoDetalhadoDto>> ListarEstoqueMovimentoDetalhadoReport(DateTime startDate, DateTime endDate, long estoqueId, long grupoId, string produto, string lote, int tipoRel = 1);

        #endregion Listar

        #region → Dropdowns
        Task<DropdownItems> ObterProdutoPorCodigoBarrasDropdown(string search);

        Task<IResultDropdownList<long>> ListarProdutoDropdown(DropdownInput dropdowninput);

        Task<IResultDropdownList<long>> ListarProdutoPorEstoqueDropdown(DropdownInput dropdowninput);

        /// <summary>
        /// Listar produtos filtrados por Grupo, paginados para uso com select2
        /// </summary>
        /// <param name="dropdownInput"></param>
        /// <returns></returns>
        Task<IResultDropdownList<long>> ListarProdutoPorGrupoDropdown(DropdownInput dropdowninput);

        /// <summary>
        ///Listar para BrasPreco, paginados para uso com select2
        /// </summary>
        /// <param name="dropdownInput"></param>
        /// <returns></returns>
        Task<IResultDropdownList<long>> ListarDropdownParaBrasPreco(DropdownInput dropdowninput);

        Task<IResultDropdownList<long>> ListarProdutoPorEstoqueIdDropdown(DropdownInput dropdowninput);

        Task<IResultDropdownList<long>> ListarMedicamentoPorEstoqueDropdown(DropdownInput dropdowninput);

        Task<IResultDropdownList<long>> ListarProdutoPorEstoque2Dropdown(DropdownInput dropdowninput);

        Task<IResultDropdownList<long>> ListarProdutoPorSaidaAtendimentoDropdown(DropdownInput dropdowninput);

        Task<IResultDropdownList<long>> ListarProdutoSaidaPorEstoqueIdEAtendimentoDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarProdutoSaidaPorEstoqueIdESetorDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarProdutoPorSaidaSetorDropdown(DropdownInput dropdowninput);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdowninput);

        Task<IResultDropdownList<long>> ListarDcbDropdown(DropdownInput dropdowninput);
        Task<IResultDropdownList<long>> ListarProdutoMestreDropdown(DropdownInput dropdownInput);

        #endregion Dropdowns

        Task<DefaultReturn<ProdutoDto>> Excluir(long produtoId);
        Task<DefaultReturn<ProdutoFornecedorDto>> ExcluirProdutoFornecedor(long produtoId, string cnpj);

        #endregion Metodos
    }
}