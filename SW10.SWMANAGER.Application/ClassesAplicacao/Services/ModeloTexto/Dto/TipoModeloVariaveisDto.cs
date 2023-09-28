// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TipoModeloVariaveisDto.cs" company="">
//   
// </copyright>
// <summary>
//   The tipo modelo variaveis dto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ModeloTexto.Dto
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;

    /// <inheritdoc />
    /// <summary>
    /// The tipo modelo variaveis dto.
    /// </summary>
    [AutoMap(typeof(TipoModeloVariaveis))]
    public class TipoModeloVariaveisDto : FullAuditedEntityDto<long>
    {
        /// <summary>
        /// Gets or sets the descricao.
        /// </summary>
        public string Descricao { get; set; }
    }
}