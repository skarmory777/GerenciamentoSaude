using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes
{
    public interface IGrauInstrucaoAppService : IApplicationService
    {
        Task<PagedResultDto<GrauInstrucaoDto>> Listar(ListarGrausInstrucoesInput input);

        Task<ListResultDto<GrauInstrucaoDto>> ListarAutoComplete(string input);

        Task CriarOuEditar(CriarOuEditarGrauInstrucao input);

        Task Excluir(CriarOuEditarGrauInstrucao input);

        Task<CriarOuEditarGrauInstrucao> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarGrausInstrucoesInput input);

    }
}
