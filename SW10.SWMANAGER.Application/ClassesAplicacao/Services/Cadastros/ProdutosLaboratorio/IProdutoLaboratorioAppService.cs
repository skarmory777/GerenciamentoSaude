using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio
{
    public interface IProdutoLaboratorioAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<ProdutoLaboratorioDto>> Listar(ListarProdutosLaboratorioInput input);

        Task<ListResultDto<ProdutoLaboratorioDto>> ListarTodos();

        Task CriarOuEditar(ProdutoLaboratorioDto input);

        Task Excluir(ProdutoLaboratorioDto input);

        Task<ProdutoLaboratorioDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosLaboratorioInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
