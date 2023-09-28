using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes
{
    public interface IFaturamentoBrasApresentacaoAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoBrasApresentacaoDto>> Listar(ListarFaturamentoBrasApresentacoesInput input);

        Task CriarOuEditar(FaturamentoBrasApresentacaoDto input);

        Task Excluir(FaturamentoBrasApresentacaoDto input);

        Task<FaturamentoBrasApresentacaoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoBrasApresentacoesInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
