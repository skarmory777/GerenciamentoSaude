using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public interface IProdutoRelacaoAcaoTerapeuticaAppService : IApplicationService
    {
        //Task<ProdutoRelacaoAcaoTerapeuticaDto> CriarOuEditar(ProdutoRelacaoAcaoTerapeuticaDto input, long id);
        Task<ProdutoRelacaoAcaoTerapeuticaDto> CriarOuEditar(ProdutoRelacaoAcaoTerapeuticaDto input);

        Task Editar(ProdutoRelacaoAcaoTerapeuticaDto input);

        Task Excluir(long id);

        Task<ProdutoRelacaoAcaoTerapeuticaDto> Obter(long id);

        Task<PagedResultDto<ProdutoRelacaoAcaoTerapeuticaDto>> ListarPorProduto(ListarInput Id);

    }
}
