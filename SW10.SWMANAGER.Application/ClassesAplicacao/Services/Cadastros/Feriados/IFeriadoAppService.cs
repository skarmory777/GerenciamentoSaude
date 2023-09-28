using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados
{
    public interface IFeriadoAppService : IApplicationService
    {
        Task<PagedResultDto<FeriadoDto>> Listar(ListarFeriadosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long? feriadoId);

        Task CriarOuEditar(CriarOuEditarFeriado input);

        Task Excluir(CriarOuEditarFeriado input);

        Task<CriarOuEditarFeriado> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFeriadosInput input);

        Task<CriarOuEditarFeriado> Obter(string uf);
    }
}
