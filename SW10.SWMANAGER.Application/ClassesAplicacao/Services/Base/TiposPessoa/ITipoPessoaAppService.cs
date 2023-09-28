using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposPessoa
{
    public interface ITipoPessoaAppService : IApplicationService
    {
        Task<ListResultDto<TipoPessoaDto>> ListarTodos();

        Task CriarOuEditar(TipoPessoaDto input);

        Task Excluir(TipoPessoaDto input);

        Task<TipoPessoaDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
