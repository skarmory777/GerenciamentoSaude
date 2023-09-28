using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos
{
    public interface IFaturamentoGrupoAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoGrupoDto>> Listar(ListarFaturamentoGruposInput input);

        Task<ResultDropdownList> ListarPorTipo(DropdownInput dropdownInput);

        Task<long> CriarOuEditar(FaturamentoGrupoDto input);

        Task Excluir(FaturamentoGrupoDto input);

        Task<FaturamentoGrupoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoGruposInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        // FATURAMENTO GRUPO CONVENIO
        Task<PagedResultDto<FaturamentoGrupoConvenioDto>> ListarConfigPorGrupo(ListarFaturamentoGruposConveniosInput input);

        Task<long> CriarOuEditarGrupoConvenio(FaturamentoGrupoConvenioDto input);

        Task<FaturamentoGrupoConvenioDto> ObterGrupoConvenio(long id);
    }
}
