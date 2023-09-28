using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento
{
    public interface IProdutoGrupoTratamentoAppService : IApplicationService
    {
        //ListResultDto<ProdutoGrupoTratamentoDto> GetProdutosGruposTratamento(GetProdutosGruposTratamentoInput input);
        Task<PagedResultDto<ProdutoGrupoTratamentoDto>> Listar(ListarProdutosGruposTratamentoInput input);

        Task CriarOuEditar(CriarOuEditarProdutoGrupoTratamento input);

        Task Excluir(CriarOuEditarProdutoGrupoTratamento input);

        Task<CriarOuEditarProdutoGrupoTratamento> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosGruposTratamentoInput input);
    }
}
