// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TamanhoModelo.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TamanhoModelo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.ModeloTexto
{
    using Abp.Domain.Entities.Auditing;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <inheritdoc />
    /// <summary>
    /// The tamanho modelo.
    /// </summary>
    [Table("TamanhoModelo")]
    public class TamanhoModelo : FullAuditedEntity<long>
    {
        /// <summary>
        /// Gets or sets the descricao.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Gets or sets the largura pixel.
        /// </summary>
        public virtual double LarguraPixel { get; set; }

        /// <summary>
        /// Gets or sets the altura pixel.
        /// </summary>
        public virtual double AlturaPixel { get; set; }

        /// <summary>
        /// Gets or sets the largura cm.
        /// </summary>
        public virtual double LarguraCm { get; set; }

        /// <summary>
        /// Gets or sets the altura cm.
        /// </summary>
        public virtual double AlturaCm { get; set; }
    }
}