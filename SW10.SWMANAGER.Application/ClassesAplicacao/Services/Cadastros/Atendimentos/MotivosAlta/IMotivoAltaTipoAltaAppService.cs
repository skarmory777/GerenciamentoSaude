using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta
{
    public interface IMotivoAltaTipoAltaAppService : IApplicationService
    {
        Task<PagedResultDto<MotivoAltaTipoAltaDto>> Listar(ListarMotivosAltaInput input);

        Task CriarOuEditar(CriarOuEditarMotivoAltaTipoAlta input);

        Task Excluir(CriarOuEditarMotivoAltaTipoAlta input);

        Task<CriarOuEditarMotivoAltaTipoAlta> Obter(long id);
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
