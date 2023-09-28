using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupos;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo
{
    public interface IFaturamentoTipoGrupoAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoTipoGrupoDto>> Listar(ListarFaturamentoTiposGrupoInput input);

        Task CriarOuEditar(FaturamentoTipoGrupoDto input);

        Task Excluir(FaturamentoTipoGrupoDto input);

        Task<FaturamentoTipoGrupoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoTiposGrupoInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
