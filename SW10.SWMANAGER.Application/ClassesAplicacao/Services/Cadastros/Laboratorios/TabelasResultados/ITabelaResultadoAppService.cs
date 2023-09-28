using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados
{
    public interface ITabelaResultadoAppService : IApplicationService
    {
        Task<PagedResultDto<TabelaResultadoDto>> Listar(ListarTabelaResultadosInput input);

        Task<PagedResultDto<TabelaResultadoDto>> ListarJson(List<TabelaResultadoDto> input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(TabelaResultadoDto input);

        Task Excluir(TabelaResultadoDto input);

        Task<TabelaResultadoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTabelaResultadosInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

    }
}
