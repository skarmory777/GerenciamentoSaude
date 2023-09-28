// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListarTamanhoModeloInput.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ListarTamanhoModeloInput type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ModeloTexto.Input
{
    using Abp.Extensions;
    using Abp.Runtime.Validation;

    using SW10.SWMANAGER.Dto;

    /// <inheritdoc cref="PagedAndSortedInputDto" />
    /// <summary>
    /// The listar tamanho modelo input.
    /// </summary>
    public class ListarTamanhoModeloInput : PagedAndSortedInputDto, IShouldNormalize
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
