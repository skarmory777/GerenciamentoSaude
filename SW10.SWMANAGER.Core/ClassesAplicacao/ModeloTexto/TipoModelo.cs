// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TipoModelo.cs" company="">
//   
// </copyright>
// <summary>
//   The tipo modelo.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.ModeloTexto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The tipo modelo.
    /// </summary>
    [Table("TipoModelo")]
    public class TipoModelo : CamposPadraoCRUD
    {
        /// <summary>
        /// Gets or sets the variaveis.
        /// </summary>
        public ICollection<TipoModeloVariaveis> Variaveis { get; set; }
    }
}
