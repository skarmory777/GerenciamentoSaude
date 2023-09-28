using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops
{
    public interface ICfopAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<CfopDto>> Listar(ListarCfopsInput input);

        Task<ListResultDto<CfopDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarCfop input);

        Task Excluir(CriarOuEditarCfop input);

        Task<CriarOuEditarCfop> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarCfopsInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<CriarOuEditarCfop> ObterPorNumero(long numero);
    }
}
