using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEstoqueLoteValidadeAppService : IApplicationService
    {
        // Task<PagedResultDto<EstoquePreMovimentoLoteValidadeDto>> ListarTodos();

        DefaultReturn<EstoquePreMovimentoLoteValidadeDto> CriarOuEditar(EstoquePreMovimentoLoteValidadeDto input);
        Task<EstoquePreMovimentoLoteValidadeDto> Obter(long id);
        Task<PagedResultDto<EstoquePreMovimentoLoteValidadeDto>> ListarPorPreMovimentoItem(ListarEstoquePreMovimentoInput input);
        Task<List<GenericoIdNome>> ObterPorProdutoEstoque(long produtoId, long estoqueId, long preMovimentoId);

        Task<IResultDropdownList<long>> ListarDropdownSaldo(DropdownInput dropdownInput);
        Task<List<GenericoIdNome>> ObterPorProdutoEstoquePorSaldo(long produtoId, long estoqueId, long preMovimentoId, long? loteValidadeId = null);
        Task Excluir(EstoquePreMovimentoLoteValidadeDto input);
        Task<List<GenericoIdNome>> ObterPorProdutoEstoqueComSaida(long produtoId, long estoqueId, long tipoMovimentoId, long? unidadeOrganizacionalId, long? pacienteId);
        Task<IResultDropdownList<long>> ListarProdutoDropdownPorLaboratorio(DropdownInput dropdownInput);
        LoteValidadeDto Obter(long produtoId, string lote, DateTime validade, long? laboratorioId);
        Task<PagedResultDto<LoteValidadeGridDto>> ListarPorProduto(LoteValidadeListarInput input);
        Task<LoteValidadeDto> ObterPorId(long id);
        Task<List<LoteValidadeGridDto>> ObterPorProdutoEstoqueLaboratorio(long produtoId, long estoqueId, long? laboratorioId);
    }
}
