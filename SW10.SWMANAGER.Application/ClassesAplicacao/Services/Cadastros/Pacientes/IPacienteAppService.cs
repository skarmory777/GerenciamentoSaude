using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes
{
    public interface IPacienteAppService : IApplicationService
    {
        Task<PagedResultDto<PacienteDto>> Listar(ListarPacientesInput input);

        Task<ListResultDto<PacienteDto>> ListarTodos();

        Task<List<Paciente>> ListarAutoComplete(string term);

        Task<PagedResultDto<ListarPacientesIndex>> ListarParaIndex(ListarPacientesInput input);

        // temp
        // Task<PagedResultDto<ListarPacientesIndex>> ListarParaIndex();

        Task<long> CriarOuEditarOriginal(PacienteDto input);

        Task<long> CriarOuEditar(PacienteDto input);

        Task Excluir(PacienteDto input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarPacientesEmAtendimentoDropdown(DropdownInput dropdownInput);

        Task<PacienteDto> Obter(long id);

        Task<PacienteDto> Obter2(long id);

        Task<PacienteDto> ObterNoMap(long id);

        Task<FileDto> ListarParaExcel(ListarPacientesInput input);

        Task<ListResultDto<VWTesteDto>> ListarResumo();

        Task<PacienteDto> ObterPorCpf(string cpf);

        Task<IResultDropdownList<long>> ListarIncluindoCPFDropdown(DropdownInput dropdownInput);
        Task<PaiMaeDto> ObterNomePaiMae(long id);
    }
}
