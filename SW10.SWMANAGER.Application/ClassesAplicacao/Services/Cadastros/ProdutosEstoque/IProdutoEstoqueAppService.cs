using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque
{
	public interface IProdutoEstoqueAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<ProdutoEstoqueDto>> Listar(ListarProdutosEstoqueInput input);

        Task CriarOuEditar(ProdutoEstoqueDto input);

        Task Excluir(ProdutoEstoqueDto input);

        Task<ProdutoEstoqueDto> Obter(long id);

        //Task<FileDto> ListarParaExcel(ListarProdutosEstoqueInput input);
   
    }
}
