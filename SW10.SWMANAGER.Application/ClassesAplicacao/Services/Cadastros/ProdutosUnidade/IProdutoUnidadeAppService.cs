using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade
{
    public interface IProdutoUnidadeAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<ProdutoUnidadeDto>> Listar(ListarProdutosUnidadeInput input);

        Task CriarOuEditar(CriarOuEditarProdutoUnidade input);

        Task Excluir(CriarOuEditarProdutoUnidade input);

        Task<ProdutoUnidadeDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosUnidadeInput input);

        Task<IResultDropdownList<long>> ListarUnidadePorProdutoDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarUnidadeComprasPorProdutoDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarUnidadePorReferenciaProdutoDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarUnidadeConsumoProdutoDropdown(DropdownInput dropdownInput);
    }
}
