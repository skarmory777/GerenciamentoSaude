// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TipoModeloVariaveis.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TipoModeloVariaveis type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.ModeloTexto
{
    using Abp.Domain.Entities.Auditing;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <inheritdoc />
    [Table("TipoModeloVariaveis")]
    public class TipoModeloVariaveis : FullAuditedEntity<long>
    {
        /// <summary>
        /// Gets or sets the tipo modelo id.
        /// </summary>
        public long TipoModeloId { get; set; }

        /// <summary>
        /// Gets or sets the tipo modelo.
        /// </summary>
        [ForeignKey("TipoModeloId")]
        public TipoModelo TipoModelo { get; set; }

        /// <summary>
        /// Gets or sets the descricao.
        /// </summary>
        public string Descricao { get; set; }
    }
}