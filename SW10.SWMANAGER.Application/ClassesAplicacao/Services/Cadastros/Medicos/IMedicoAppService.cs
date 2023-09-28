using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos
{
    public interface IMedicoAppService : IApplicationService
    {
        Task<PagedResultDto<ListarMedicoIndex>> Listar(ListarMedicosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task<ListResultDto<MedicoDto>> ListarTodos();

        Task<MedicoDto> CriarOuEditar(MedicoDto input);

        Task Excluir(MedicoDto input);

        Task<IEnumerable<MedicoDto>> ObterIds(List<long> ids);

        Task<MedicoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarMedicosInput input);

        Task<ICollection<MedicoDto>> ListarPorEspecialidade(long id);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownFatContaItem(DropdownInput dropdownInput);

        Task<MedicoDto> ObterPorCPF(string cpf);
    }
}
