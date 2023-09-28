using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento
{
    public interface IProdutoCodigoMedicamentoAppService : IApplicationService
    {
        //ListResultDto<ProdutoCodigoMedicamentoDto> GetProdutosCodigosMedicamento(GetProdutosCodigosMedicamentoInput input);
        Task<PagedResultDto<ProdutoCodigoMedicamentoDto>> Listar(ListarProdutosCodigosMedicamentoInput input);

        Task<ListResultDto<ProdutoCodigoMedicamentoDto>> ListarTodos();

        Task CriarOuEditar(ProdutoCodigoMedicamentoDto input);

        Task Excluir(ProdutoCodigoMedicamentoDto input);

        Task<ProdutoCodigoMedicamentoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosCodigosMedicamentoInput input);
    }
}
