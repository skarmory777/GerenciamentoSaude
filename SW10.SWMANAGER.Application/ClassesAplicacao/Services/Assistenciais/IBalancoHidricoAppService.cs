// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBalancoHidricoAppService.cs" company="">
//   
// </copyright>
// <summary>
//   The BalancoHidricoAppService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Application.Services;
    using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.BalancoHidrico;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
    using System;
    using System.Threading.Tasks;

    /// <inheritdoc />
    public interface IBalancoHidricoAppService : IApplicationService
    {
        /// <summary>
        /// The obter.
        /// </summary>
        /// <param name="atendId">
        /// The atend id.
        /// </param>
        /// <param name="dataBalanco">
        /// The data balanco.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<BalancoHidricoDto> ObterAsync(long atendId, DateTime dataBalanco);


        Task<BalancoHidricoDto> ObterIdAsync(long id);

        Task<BalancoHidricoDto> ObterBalancoHidricoAnterior(long atendId, long? balancoHidricoId);

        Task<long> ObterIdBalancoHidricoAnterior(long atendId, long? balancoHidricoId);

        Task<long> ObterIdBalancoHidricoAnterior(long atendId, DateTime dateBalancoHidrico);

        /// <summary>
        /// The gerar novo balanco hidrico.
        /// </summary>
        /// <param name="atendId">
        /// The atend id.
        /// </param>
        /// <param name="dataBalanco">
        /// The data balanco.
        /// </param>
        /// <param name="horaIntervalo">
        /// The hora Intervalo.
        /// </param>
        /// <param name="numSolucoes">
        /// The num Solucoes.
        /// </param>
        /// <returns>
        /// The <see cref="BalancoHidricoDto"/>.
        /// </returns>
        BalancoHidricoDto GerarNovoBalancoHidrico(long atendId, DateTime dataBalanco, int horaIntervalo, int numSolucoes);

        /// <summary>
        /// The up sert balanco hidrico.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpSertBalancoHidricoAsync(BalancoHidricoDto model);

        /// <summary>
        /// The obter balanco 24 hrs.
        /// </summary>
        /// <param name="modelId">
        /// The model id.
        /// </param>
        /// <param name="dateValue">
        /// The date value.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<BalancoHidrico24HrsViewModel> ObterBalanco24HrsAsync(long modelId, DateTime dateValue);


        Task<bool> Conferir(long balancoHidricoId);

        Task<bool> Desconferir(long balancoHidricoId);
    }
}
