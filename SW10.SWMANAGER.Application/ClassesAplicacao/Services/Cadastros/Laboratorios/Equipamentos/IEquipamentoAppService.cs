using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos
{
    public interface IEquipamentoAppService : IApplicationService
    {
        Task<PagedResultDto<EquipamentoDto>> Listar(ListarEquipamentosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(EquipamentoDto input);

        Task Excluir(EquipamentoDto input);

        Task<EquipamentoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarEquipamentosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
