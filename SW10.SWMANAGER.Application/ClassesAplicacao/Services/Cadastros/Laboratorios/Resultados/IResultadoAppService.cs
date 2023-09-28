using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Input;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados
{
    public interface IResultadoAppService : IApplicationService
    {
        Task<PagedResultDto<ResultadoIndexDto>> Listar(ListarResultadosInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task<ResultadoDto> CriarOuEditar(ResultadoDto input);

        Task Excluir(ResultadoDto input);

        Task<ResultadoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarResultadosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
        Task<PagedResultDto<ResultadoColetaIndexDto>> ListarExamesPorColeta(ResultadoColetaInput input);
        Task<PagedResultDto<FormatacaoItemIndexDto>> ListarItensFormatacaoExame(LaudoResultadoInput input);
        Task<ResultadoExameDto> ObterResultadoExame(long id);
        Task<List<FormatacaoItemIndexDto>> ListarItensFormatacaoPorExame(LaudoResultadoInput input);
        Task<List<FormatacaoItemIndexDto>> ListarItensFormatacaoPorColeta(LaudoResultadoInput input);
        Task<PagedResultDto<ResultadoIndexDto>> ListarNaoConferido(ListarResultadosInput input);
        RegistroArquivoDto ObterArquivoExameColeta(long coletaId);

        Task<ResultadoDto> ObterPorSolicitacaoExameId(long solicitacaoExameId);

        Task<PagedResultDto<ResultadoColetaDetalhamentoIndexDto>> ObterResultadoExamesPorResultadoId(
            ResultadoColetaDetalhamentoIndexFilterDto input);

        Task AtualizaStatus(long resultadoId);

    }
}
