using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NFe.Classes;

using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEstoqueImportacaoProdutoAppService : IApplicationService
    {

        Task<PagedResultDto<EstoqueImportacaoProdutoListarDto>> Listar(ListarInput input);

        Task<EstoqueImportacaoProdutoListarDto> CriarOuEditar(EstoqueImportacaoProdutoListarDto input);

        List<EstoqueImportacaoProdutoDto> ObterListaImportacaoProduto(nfeProc nf);
        DefaultReturn<Object> RelacionarProdutos(List<EstoqueImportacaoProdutoDto> importacaoProdutos, long fornecedorId, string CNPJNota);
        DateTime ObterValidade(string informacaoAdicional);
    }
}
