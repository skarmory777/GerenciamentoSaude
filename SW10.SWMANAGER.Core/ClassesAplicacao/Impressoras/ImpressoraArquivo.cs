// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImpressoraArquivo.cs" company="">
//   
// </copyright>
// <summary>
//   The impressora arquivo.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Impressoras
{
    using Abp.Domain.Entities.Auditing;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The impressora arquivo.
    /// </summary>
    [Table("SisImpressoraArquivo")]
    public class ImpressoraArquivo : FullAuditedEntity<long>
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
