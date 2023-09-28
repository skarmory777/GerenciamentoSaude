using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas
{
    public interface IFormataAppService : IApplicationService
    {
        Task<PagedResultDto<FormataDto>> Listar(ListarFormatasInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(FormataDto input);

        Task Excluir(FormataDto input);

        Task<FormataDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFormatasInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
