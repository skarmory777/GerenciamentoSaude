using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta
{
    public interface IMotivoAltaAppService : IApplicationService
    {
        Task<PagedResultDto<MotivoAltaDto>> Listar(ListarMotivosAltaInput input);

        Task CriarOuEditar(CriarOuEditarMotivoAlta input);

        Task Excluir(CriarOuEditarMotivoAlta input);

        Task<CriarOuEditarMotivoAlta> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarMotivosAltaInput input);
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
