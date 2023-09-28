using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos
{
    public interface IResultadoLaudoAppService : IApplicationService
    {
        Task<PagedResultDto<ResultadoLaudoDto>> Listar(ListarResultadoLaudosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(ResultadoLaudoDto input);

        Task Excluir(ResultadoLaudoDto input);

        Task<ResultadoLaudoDto> Obter(long id);

        //Task<FileDto> ListarParaExcel (ListarResultadoLaudosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<DefaultReturn<ResultadoLaudoDto>> CriarOuEditarLista(string input, long coletaId);

        Task<PagedResultDto<ExameResultadoDto>> ListarHistorioExamePorPaciente(ExameResultadoInput input);

        Task<long> GerarArquivo(long coletaId);

        Task<string> FormatacaoColetaExame(long coletaId, long exameId);

        Task<string> FormatacaoColeta(long coletaId);
    }
}
