using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos
{
    public interface IPlanoAppService : IApplicationService
    {
        Task<PagedResultDto<PlanoDto>> Listar(ListarPlanosInput input);

        Task<PagedResultDto<PlanoDto>> ListarPorConvenio(ListarPlanosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long? convenioId);

        Task<IResultDropdownList<long>> ListarPorConvenioDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarPorConvenioExclusivoDropdown(DropdownInput dropdownInput);

        Task<ListResultDto<PlanoDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarPlano input);

        Task Excluir(CriarOuEditarPlano input);

        Task<CriarOuEditarPlano> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarPlanosInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<GenericoIdNome> ObterSomenteUmPlano(long convenioId);
    }
}
