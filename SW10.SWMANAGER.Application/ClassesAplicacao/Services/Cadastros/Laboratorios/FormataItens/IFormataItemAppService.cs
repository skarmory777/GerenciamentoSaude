using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems
{
    public interface IFormataItemAppService : IApplicationService
    {
        Task<PagedResultDto<FormataItemDto>> Listar(ListarFormataItemsInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task<DefaultReturn<FormataItemDto>> CriarOuEditar(FormataItemDto input);

        Task Excluir(FormataItemDto input);

        Task<FormataItemDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFormataItemsInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<string> CalcularFormula(string input, long itemResultadoId);

        Task<PagedResultDto<FormataItemDto>> ListarJson(List<FormataItemDto> input);

    }
}
