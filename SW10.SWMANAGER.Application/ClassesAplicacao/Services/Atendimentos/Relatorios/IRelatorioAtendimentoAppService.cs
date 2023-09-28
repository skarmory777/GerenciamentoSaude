using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Relatorios
{
    /// <summary>
    /// Serviço que provê os dados dos filtros do relatório de suplementos
    /// </summary>
    public interface IRelatorioAtendimentoAppService : IApplicationService
    {
        /// <summary>
        /// Lista dos Grupos
        /// </summary>
        /// <returns></returns>
        /// 
        IList<GenericoIdNome> ListarEmpresaUsuario(long id);

        Task<ListResultDto<AtendimentoDto>> ListarRelatorio(long empresaId, long convenioId, long especialidadeId, long medicoId);

        /// <summary>
        /// Lista para Relatorio de Atendimentos.
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<AtendimentoDetalhadoDsDto>> ListarRelatorioAtendimento(long empresaId, long convenioId, long especialidadeId, long medicoId, DateTime StartDate, DateTime EndDate);



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