using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa
{
    public interface IProdutoEmpresaAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<ProdutoEmpresaDto>> Listar(ListarProdutosEmpresaInput input);

        //Task CriarOuEditar(CriarOuEditarProdutoEmpresa input);

        Task CriarOuEditar(ProdutoEmpresaDto input);

        //Task Excluir(CriarOuEditarProdutoEmpresa input);

        Task Excluir(ProdutoEmpresaDto input);

        Task<ProdutoEmpresaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosEmpresaInput input);


    }
}
