// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModeloTextoAppService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ITipoModeloAppService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ModeloTexto
{
    using Abp.Application.Services;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using SW10.SWMANAGER.ClassesAplicacao.Services.ModeloTexto.Dto;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <inheritdoc />
    public interface ITipoModeloAppService : IApplicationService
    {
        /// <summary>
        /// The obter.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<TipoModeloDto> ObterAsync(long id);

        /// <summary>
        /// The obert variaveis por tipo modelo async.
        /// </summary>
        /// <param name="tipoModeloId">
        /// The tipo Modelo Id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<TipoModeloVariaveisDto>> ObterVariaveisPorTipoModeloAsync(long tipoModeloId);

        /// <summary>
        /// The listar.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<ResultDropdownList> ListarDropdownAsync(DropdownInput input);

        /// <summary>
        /// The listar tamanho modelo async.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<ResultDropdownList> ListarTamanhoModeloDropdownAsync(DropdownInput input);

        /// <summary>
        /// The obter tamanho async.
        /// </summary>
        /// <param name="tamanhoId">
        /// The tamanho id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<TamanhoModeloDto> ObterTamanhoAsync(long? tamanhoId);
    }
}