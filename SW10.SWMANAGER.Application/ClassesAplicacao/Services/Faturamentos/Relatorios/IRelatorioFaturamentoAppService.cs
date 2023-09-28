using Abp.Application.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Relatorios
{
    /// <summary>
    /// Serviço que provê os dados dos filtros do relatório de suplementos
    /// </summary>
    public interface IRelatorioFaturamentoAppService : IApplicationService
    {
        //  Task<byte[]> GuiaResumoInternacaoPdf(long atendimentoId, string reportPath, dynamic resumo_internacao_dataset);

        /// <summary>
        /// Lista dos Grupos
        /// </summary>
        /// <returns></returns>
        /// 
        //   IList<GenericoIdNome> ListarEmpresaUsuario(long id);

        //    Task<ListResultDto<FaturamentoDto>> ListarRelatorio(long empresaId);
        /// <summary>
        /// Lista dos GrupoClasse
        /// </summary>
        /// <param name="filtro">Grupo Pai</param>
        /// <returns></returns>
        // Task<IList<GenericoIdNome>> Listar(Grupo filtro);

        ///// <summary>
        ///// Lista de GrupoSubClasse
        ///// </summary>
        ///// <param name="filtro">GrupoClasse pai</param>
        ///// <returns></returns>
        //Task<IList<GenericoIdNome>> Listar(GrupoClasse filtro);

        //IList<RelatorioMovimentacaoItemDto> DadosRelatorioMovimentacao(RelatorioMovimentacaoFiltroDto filtro);
    }
}