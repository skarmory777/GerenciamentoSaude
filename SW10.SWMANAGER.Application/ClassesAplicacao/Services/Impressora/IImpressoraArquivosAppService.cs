// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImpressoraArquivosAppService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IImpressoraArquivosAppService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Impressora
{
    using Abp.Application.Services;
    using Abp.Authorization;
    using SW10.SWMANAGER.ClassesAplicacao.Impressoras;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The ImpressoraArquivosAppService interface.
    /// </summary>
    public interface IImpressoraArquivosAppService : IApplicationService
    {
        /// <summary>
        /// The listar arquivos pendentes impressao.
        /// </summary>
        /// <param name="TenancyName">
        /// The tenancy name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [AbpAllowAnonymous]
        Task<IEnumerable<ImpressoraArquivoDto>> ListarArquivosPendentesImpressao(string TenancyName);

        /// <summary>
        /// The arquivo impresso sucesso.
        /// </summary>
        /// <param name="tenancyName">
        /// The tenancy name.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [AbpAllowAnonymous]
        Task<ImpressoraArquivoDto> ArquivoImpressoSucesso(string tenancyName, long id);

        /// <summary>
        /// The enviar para imprimir.
        /// </summary>
        /// <param name="printerName">
        /// The printer Name.
        /// </param>
        /// <param name="modelo">
        /// The modelo.
        /// </param>
        /// <param name="texto">
        /// The texto.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="numberOfCopies">
        /// The number of copies.
        /// </param>
        /// <returns>
        /// The <see cref="ImpressoraArquivo"/>.
        /// </returns>
        [AbpAllowAnonymous]
        ImpressoraArquivo EnviarParaImprimir(
            string printerName,
            byte[] file,
            string fileName,
            long numberOfCopies = 1);
    }
}