using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public interface IProdutoRelacaoLaboratorioAppService : IApplicationService
    {
        //Task<ProdutoRelacaoLaboratorioDto> CriarOuEditar(ProdutoRelacaoLaboratorioDto input, long id);
        Task<ProdutoRelacaoLaboratorioDto> CriarOuEditar(ProdutoRelacaoLaboratorioDto input);

        Task Editar(ProdutoRelacaoLaboratorioDto input);

        Task Excluir(long id);

        Task<ProdutoRelacaoLaboratorioDto> Obter(long id);

        Task<PagedResultDto<ProdutoRelacaoLaboratorioDto>> ListarPorProduto(ListarInput Id);
    }
}
