using System;
using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios
{
    /// <summary>
    /// Serviço que provê os dados dos filtros do relatório de suplementos
    /// </summary>
    public interface IRelatorioSuprimentoAppService : IApplicationService
    {
        /// <summary>
        /// Lista dos Grupos
        /// </summary>
        /// <returns></returns>
        Task<IList<GenericoIdNome>> Listar();

        /// <summary>
        /// Lista dos GrupoClasse
        /// </summary>
        /// <param name="filtro">Grupo Pai</param>
        /// <returns></returns>
        Task<IList<GenericoIdNome>> Listar(Grupo filtro);

        /// <summary>
        /// Lista de GrupoSubClasse
        /// </summary>
        /// <param name="filtro">GrupoClasse pai</param>
        /// <returns></returns>
        Task<IList<GenericoIdNome>> Listar(GrupoClasse filtro);

        IList<RelatorioMovimentacaoItemDto> DadosRelatorioMovimentacao(RelatorioMovimentacaoFiltroDto filtro);


        Task<byte[]> RetornaConsumoPorPaciente(DateTime dataInicio, DateTime dataFinal, long? pacienteId,
            long? empresaId);
        
        Task<byte[]> RetornaConsumoPorSetor(DateTime dataInicio, DateTime dataFinal, long? unidadeOrganizacionalId,
            long? empresaId);
        
        Task<byte[]> RetornaDevolucaoPorEstoque(DateTime dataInicio, DateTime dataFinal, long? estoqueId,
            long? empresaId);
        
        Task<byte[]> RetornaDevolucaoPorPaciente(DateTime dataInicio, DateTime dataFinal, long? estoqueId,
            long? empresaId, long? pacienteId);
        
        Task<byte[]> RetornaPerdaPorEstoque(DateTime dataInicio, DateTime dataFinal, long? estoqueId,
            long? empresaId);
        
        Task<byte[]> RetornaUltimasCompras(RelatorioUltimasComprasDto input);

        Task<byte[]> RetornaUltimasComprasVsAtual(RelatorioUltimasComprasVsAtualDto input);

        Task<byte[]> RetornaAcuracia(RelatorioAcuraciaDto input);

        Task<byte[]> RetornaMapaDispensacao(DateTime dataInicio, DateTime dataFinal, long? unidadeId, long? empresaId);
    }
}