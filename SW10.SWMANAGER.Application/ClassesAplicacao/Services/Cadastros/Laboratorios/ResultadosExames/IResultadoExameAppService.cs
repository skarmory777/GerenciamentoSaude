using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames
{
    public interface IResultadoExameAppService : IApplicationService
    {
        Task<PagedResultDto<ResultadoExameIndexCrudDto>> Listar(ListarInput input);

        Task<ListResultDto<ResultadoExameIndexCrudDto>> ListarPorResultado(long id);

        Task<PagedResultDto<ResultadoExameIndexDto>> ListarIndex(ListarInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(ResultadoExameDto input);

        Task Excluir(ResultadoExameDto input);

        Task<ResultadoExameDto> Obter(long id);

        Task<ListResultDto<ExameStatusDto>> Legenda();

        Task<FileDto> ListarParaExcel(ListarInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<PagedResultDto<ResultadoExameIndexCrudDto>> ListarJson(List<ResultadoExameIndexCrudDto> list);

        Task<PagedResultDto<ResultadoExameIndexCrudDto>> ListarNaoConferidos(ListarResultadoExamesInput input);

        Task<DefaultReturn<ResultadoExameIndexCrudDto>> RegistrarConferenciaExames(long[] examesIds);

        Task<IEnumerable<ResultadoExameDto>> ObterResultadoExames(long resultadoId, List<long> resultadoExameIds = null);
        
        Task<IEnumerable<ResultadoExameDto>> ObterResultadoExamesPorSolicitacaoExames(long resultadoId, List<long> solicitacaoExameIds = null);

        Task<DefaultReturn<object>> RegistrarBaixa(long resultadoId,  long? tecnicoId, List<LabResultadoExameBaixaInputDto> resultadoExamesBaixa);

        Task AdicionarPendencias(long resultadoId, List<LabResultadoExameBaixaInputDto> resultadoExamesPendencias);

        Task<DefaultReturn<object>> VoltarStatusAnterior(long resultadoId, List<long> resultadoExameIds);
        
        Task<DefaultReturn<object>> AdicionarExameColeta(long resultadoId, long solicitacaoExameId, List<long> solicitacaoExameIds);

        Task AtualizarObservacao(long resultadoExameId, string observacao);

        Task ResolverPendencias(long resultadoId, List<long> resultadoExameIds);

        Task ImprimirEtiquetas(LabImprimirEtiquetaInputDto input);
    }
}
