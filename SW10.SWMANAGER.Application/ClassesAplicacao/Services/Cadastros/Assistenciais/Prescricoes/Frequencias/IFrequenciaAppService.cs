using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias
{
    public interface IFrequenciaAppService : IApplicationService
    {
        Task<PagedResultDto<FrequenciaDto>> Listar(ListarInput input);

        Task<ListResultDto<FrequenciaDto>> ListarTodos();

        Task<ListResultDto<FrequenciaDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(ConfiguracaoPrescricaoItemDropDownInput dropdownInput);

        Task<FrequenciaDto> CriarOuEditar(FrequenciaDto input);

        Task Excluir(FrequenciaDto input);

        Task<FrequenciaDto> Obter(long id);

        FrequenciaDto ObterSync(long id);

        Task<IEnumerable<FrequenciaDto>> ObterIds(List<long> ids);

        Task<FileDto> ListarParaExcel(ListarInput input);
    }
}
