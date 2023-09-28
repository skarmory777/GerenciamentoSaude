using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Inputs
{
    public class ListarOrdensCompraInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public string OrdenarPor { get; set; }
        public string EmpresaId { get; set; }
        public string UnidadeOrganizacionalId { get; set; }
        public string OrdemCompraStatusId { get; set; }
        public string Codigo { get; set; }
        public string Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public void Normalize()
        {
            Sorting = OrdenarPor;
        }

    }
}
