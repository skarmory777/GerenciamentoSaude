// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TamanhoModeloDto.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TamanhoModeloDto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ModeloTexto.Dto
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;

    /// <inheritdoc />
    /// <summary>
    /// The tamanho modelo dto.
    /// </summary>
    [AutoMap(typeof(TamanhoModelo))]
    public class TamanhoModeloDto : FullAuditedEntityDto<long>
    {
        /// <summary>
        /// Gets or sets the descricao.
        /// </summary>
        public virtual string Descricao { get; set; }

        public virtual double LarguraPixel { get; set; }

        public virtual double AlturaPixel { get; set; }

        public virtual double LarguraCm { get; set; }

        public virtual double AlturaCm { get; set; }
    }
}