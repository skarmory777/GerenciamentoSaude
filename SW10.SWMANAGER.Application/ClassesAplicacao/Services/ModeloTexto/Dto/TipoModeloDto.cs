// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TipoModeloDto.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TipoModeloDto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ModeloTexto.Dto
{
    using System.Collections.Generic;

    /// <inheritdoc />
    public class TipoModeloDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Gets or sets the variaveis.
        /// </summary>
        public ICollection<TipoModeloVariaveisDto> Variaveis { get; set; }
    }
}
