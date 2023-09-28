// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListarTipoModeloInput.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ListarTipoModeloInput type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ModeloTexto.Input
{
    using Abp.Extensions;
    using Abp.Runtime.Validation;

    using SW10.SWMANAGER.Dto;

    /// <inheritdoc cref="PagedAndSortedInputDto" />
    public class ListarTipoModeloInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// Gets or sets the filtro.
        /// </summary>
        public string Filtro { get; set; }

        /// <inheritdoc />
        public virtual void Normalize()
        {
            if (this.Sorting.IsNullOrWhiteSpace())
            {
                this.Sorting = "Descricao";
            }
        }
    }
}
