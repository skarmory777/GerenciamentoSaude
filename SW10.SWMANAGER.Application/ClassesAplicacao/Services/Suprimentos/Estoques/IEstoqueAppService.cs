using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    public interface IEstoqueAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<EstoqueDto>> Listar(ListarProdutosEstoqueInput input);

        //Task CriarOuEditar(EstoqueDto input);
        //Task CriarOuEditar(EstoqueDto input, IList<EstoqueGrupoDto> estoquesGrupo);
        Task CriarOuEditar(EstoqueDto input);

        Task Excluir(EstoqueDto input);

        Task<EstoqueDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosEstoqueInput input);

        Task<ListResultDto<EstoqueDto>> ListarTodos();

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task<PagedResultDto<EstoqueGrupoDto>> ListarEstoqueGrupo(ListarEstoqueGrupoInput input);

        Task<IResultDropdownList<long>> ResultDropdownList(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

    }
}
