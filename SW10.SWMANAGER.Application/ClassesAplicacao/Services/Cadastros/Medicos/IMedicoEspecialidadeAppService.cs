using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos
{
    public interface IMedicoEspecialidadeAppService : IApplicationService
    {
        Task CriarOuEditar(MedicoEspecialidadeDto input);

        Task Excluir(MedicoEspecialidadeDto input);

        Task<MedicoEspecialidadeDto> Obter(long id);

        Task<PagedResultDto<MedicoEspecialidadeDto>> Listar(ListarInput input);

        Task<PagedResultDto<MedicoEspecialidadeDto>> ListarJson(List<MedicoEspecialidadeDto> list);

        Task<ListResultDto<MedicoEspecialidadeDto>> ListarMedicoEspecialidadePorMedico(long id);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownPorMedico(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownPorMedicoTodas(DropdownInput dropdownInput);

        Task<PagedResultDto<MedicoEspecialidadeDto>> ListarPorMedico(ListarInput input);

        Task<GenericoIdNome> ObterSomenteUmaEspecialidade(long medicoId);

    }
}
