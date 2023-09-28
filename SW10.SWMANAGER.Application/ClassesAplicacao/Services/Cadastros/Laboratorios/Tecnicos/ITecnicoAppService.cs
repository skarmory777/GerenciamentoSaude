using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos
{
    public interface ITecnicoAppService : IApplicationService
    {
        Task<PagedResultDto<TecnicoDto>> Listar(ListarTecnicosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(TecnicoDto input);

        Task Excluir(TecnicoDto input);

        Task<TecnicoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTecnicosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
