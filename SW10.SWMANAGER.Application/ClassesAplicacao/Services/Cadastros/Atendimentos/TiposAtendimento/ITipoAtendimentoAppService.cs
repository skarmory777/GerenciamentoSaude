using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento
{
    public interface ITipoAtendimentoAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<TipoAtendimentoDto>> Listar(ListarTiposAtendimentoInput input);

        Task CriarOuEditar(TipoAtendimentoDto input);

        Task Excluir(TipoAtendimentoDto input);

        Task<TipoAtendimentoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposAtendimentoInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
