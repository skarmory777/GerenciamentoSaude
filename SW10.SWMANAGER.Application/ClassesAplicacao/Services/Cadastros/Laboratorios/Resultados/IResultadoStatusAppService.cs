using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados
{
    public interface IResultadoStatusAppService : IApplicationService
    {
        Task<PagedResultDto<ResultadoStatusDto>> Listar(ListarInput input);

        Task CriarOuEditar(ResultadoStatusDto input);

        Task Excluir(ResultadoStatusDto input);

        Task<ResultadoStatusDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
