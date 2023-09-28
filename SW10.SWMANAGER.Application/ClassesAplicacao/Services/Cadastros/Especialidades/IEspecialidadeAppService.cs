using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades
{
    public interface IEspecialidadeAppService : IApplicationService
    {
        Task<PagedResultDto<EspecialidadeDto>> ListarEspecialidades(ListarEspecialidadesInput input);

        Task CriarOuEditar(EspecialidadeDto input);

        Task Excluir(EspecialidadeDto input);

        //Task<CriarOuEditarEspecialidade> Obter(long id);
        Task<EspecialidadeDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarEspecialidadesInput input);

        Task<ICollection<EspecialidadeDto>> ListarPorMedico(long id);

        Task<ListResultDto<EspecialidadeDto>> ListarTodos();

        Task<ListResultDto<EspecialidadeDto>> Listar(List<long> ids);
        //Task<ListResultDto<Especialidade>> Listar(List<long> ids);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<ResultDropdownList> ListarDropdownPorMedicoTodas(DropdownInput dropdownInput);
    }
}
