using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores
{
    public interface IFornecedorAppService : IApplicationService
    {
        Task<PagedResultDto<SisFornecedorIndexViewModel>> ListarFornecedores(ListarFornecedoresInput input);

        Task CriarOuEditar(SisFornecedorDto input);

        Task Excluir(CriarOuEditarFornecedor input);

        Task<SisFornecedorDto> Obter(long id);

        // Task<FileDto> ListarParaExcel(ListarFornecedoresInput input);

        //Task<ListResultDto<CriarOuEditarFornecedor>> ListarTodos();

        //Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        //Task<ICollection<FornecedorDto>> ListarPorMedico(long id);

        //Task<ListResultDto<FornecedorDto>> Listar(List<long> ids);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<SisFornecedorDto> ObterPorCNPJ(string cnpj);

        Task<SisFornecedorDto> ObterPorCPF(string cpf);

        Task<IResultDropdownList<long>> ListarDropdownSis(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownSisFornecedor(DropdownInput dropdownInput);

    }
}
