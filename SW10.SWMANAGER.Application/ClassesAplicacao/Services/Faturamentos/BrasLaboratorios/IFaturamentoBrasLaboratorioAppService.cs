using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios
{
    public interface IFaturamentoBrasLaboratorioAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoBrasLaboratorioDto>> Listar(ListarFaturamentoBrasLaboratoriosInput input);

        Task CriarOuEditar(FaturamentoBrasLaboratorioDto input);

        Task Excluir(FaturamentoBrasLaboratorioDto input);

        Task<FaturamentoBrasLaboratorioDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoBrasLaboratoriosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
