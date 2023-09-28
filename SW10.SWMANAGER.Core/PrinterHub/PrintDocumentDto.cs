// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrinterDto.cs" company="">
//   
// </copyright>
// <summary>
//   The printer dto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.PrinterHub
{
    /// <summary>
    /// The printer dto.
    /// </summary>
    public class PrintDocumentDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDocumentDto"/> class.
        /// </summary>
        public PrintDocumentDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDocumentDto"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="printerName">
        /// The printer name.
        /// </param>
        /// <param name="numberOfCopies">
        /// The number Of Copies.
        /// </param>
        public PrintDocumentDto(string fileName, byte[] file, string printerName, long numberOfCopies)
        {
            this.FileName = fileName;
            this.File = file;
            this.PrinterName = printerName;
            this.NumberOfCopies = numberOfCopies;
        }

        /// <summary>
        /// Gets or sets the number of copies.
        /// </summary>
        public long NumberOfCopies { get; set; } = 1;

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        public byte[] File { get; set; }

        /// <summary>
        /// Gets or sets the printer name.
        /// </summary>
        public string PrinterName { get; set; }
    }
}