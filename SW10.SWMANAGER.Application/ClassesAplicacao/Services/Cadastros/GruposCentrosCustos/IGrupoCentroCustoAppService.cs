using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos
{
    public interface IGrupoCentroCustoAppService : IApplicationService
    {
        Task<PagedResultDto<GrupoCentroCustoDto>> Listar(ListarGrupoCentroCustoInput input);

        Task CriarOuEditar(CriarOuEditarGrupoCentroCusto input);

        Task Excluir(CriarOuEditarGrupoCentroCusto input);

        Task<CriarOuEditarGrupoCentroCusto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
