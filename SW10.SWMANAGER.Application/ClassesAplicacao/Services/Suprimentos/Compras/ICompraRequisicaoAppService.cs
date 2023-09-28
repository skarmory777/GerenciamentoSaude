using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Inputs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ComprasRequisicao
{
    public interface ICompraRequisicaoAppService : IApplicationService
    {
        #region ↓ Atributos

        #region → Basico - CRUD
        Task<CompraRequisicaoDto> CriarOuEditar(CompraRequisicaoDto input);

        Task Excluir(CompraRequisicaoDto input);

        Task<CompraRequisicaoDto> Obter(long id);

        Task<CompraRequisicaoDto> AprovarOuRecusarRequisicao(CompraRequisicaoDto input);

        void VoltarRequisicaoStatusInicial(long requisicaoId);

        void SalvarOuAtualizarDadosFornecedorProduto(CompraCotacaoFornecedorDto input);

        void EnviarCotacaoBionexo(long[] compraCotacaoIds);
        #endregion

        #region → Basico - Listar
        Task<PagedResultDto<CompraRequisicaoIndexDto>> Listar(ListarRequisicoesCompraInput input);

        Task<PagedResultDto<CompraRequisicaoIndexDto>> ListarCotacao(ListarRequisicoesCompraInput input);

        Task<ListResultDto<CompraRequisicaoDto>> ListarTodos();
        #endregion

        #region → Gets
        Task<GenericoIdNome> ObterModoRequisicaoManual();

        Task<GenericoIdNome> ObterModoRequisicaoAutomatico();

        Task<GenericoIdNome> ObterMotivoPedidoReposicaoEstoque();
        #endregion

        #region → Requisicoes Itens
        Task<ListResultDto<CompraRequisicaoItemDto>> ListarRequisicaoItem(long id);

        Task<PagedResultDto<CompraRequisicaoItemDto>> ListarItensJson(List<CompraRequisicaoItemDto> list);

        Task<ListResultDto<CompraRequisicaoItemDto>> ListarRequisicaoAutomatica(ListarRequisicoesCompraInput input);

        Task<PagedResultDto<CompraCotacaoFornecedorDto>> ListarCotacaoFornecedorItem(long id, long? fornecedorId);
        #endregion

        #region → Dropdowns
        Task<IResultDropdownList<long>> ListarMotivoPedidoDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarAprovacaoStatusDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarTipoRequisicaoDropdown(DropdownInput dropdownInput);
        #endregion

        #region Relatorios
        byte[] GerarRelatorioCompraRequisicao(CompraRequisicaoRelatorioDto input);
        #endregion

        #endregion
    }
}
