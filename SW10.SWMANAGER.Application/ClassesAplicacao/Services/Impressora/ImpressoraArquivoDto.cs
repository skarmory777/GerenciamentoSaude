// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImpressoraArquivoDto.cs" company="">
//   
// </copyright>
// <summary>
//   The impressora arquivo dto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Impressora
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using SW10.SWMANAGER.ClassesAplicacao.Impressoras;

    /// <summary>
    /// The impressora arquivo dto.
    /// </summary>
    [AutoMap(typeof(ImpressoraArquivo))]
    public class ImpressoraArquivoDto : FullAuditedEntityDto<long>
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the printer name.
        /// </summary>
        public string PrinterName { get; set; }

        /// <summary>
        /// Gets or sets the number of copies.
        /// </summary>
        public long NumberOfCopies { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value indicating whether is printed.
        /// </summary>
        public bool IsPrinted { get; set; }
    }
}