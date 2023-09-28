using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos
{
    public interface IFaturamentoSubGrupoAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoSubGrupoDto>> Listar(ListarFaturamentoSubGruposInput input);

        Task<ResultDropdownList> ListarParaGrupo(DropdownInput dropdownInput);

        Task<ResultDropdownList> ListarParaGrupoObrigatorio(DropdownInput dropdownInput);

        Task CriarOuEditar(FaturamentoSubGrupoDto input);

        Task Excluir(FaturamentoSubGrupoDto input);

        Task<FaturamentoSubGrupoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoSubGruposInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
