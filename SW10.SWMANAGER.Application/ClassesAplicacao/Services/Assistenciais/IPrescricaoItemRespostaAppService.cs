using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.PrescricaoItemRespostaAppService;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface IPrescricaoItemRespostaAppService : IApplicationService
    {
        Task<ListResultDto<PrescricaoItemRespostaDto>> ListarTodos();

        Task<ListResultDto<PrescricaoItemRespostaDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<PrescricaoItemRespostaDto> CriarOuEditar(PrescricaoItemRespostaDto input,  bool atualizaOuCriaArquivo = true);

        Task Excluir(PrescricaoItemRespostaDto input);

        void ExcluirSync(PrescricaoItemRespostaDto input);

        Task<PagedResultDto<PrescricaoItemRespostaDto>> Listar(ListarInput input);

        Task<PrescricaoItemRespostaDto> Obter(long id);

        Task<PrescricaoItemRespostaDto> ObterJson(List<PrescricaoItemRespostaDto> list, long idGrid, long idDivisao);

        Task<FileDto> ListarParaExcel(ListarInput input);

        Task Suspender(long id);
    }
}
