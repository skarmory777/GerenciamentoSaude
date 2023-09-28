using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados
{
    public interface ITipoResultadoAppService : IApplicationService
    {
        Task<PagedResultDto<TipoResultadoDto>> Listar(ListarTipoResultadosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(TipoResultadoDto input);

        Task Excluir(TipoResultadoDto input);

        Task<TipoResultadoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTipoResultadosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
