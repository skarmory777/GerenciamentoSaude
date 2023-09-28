using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NotasFiscais.Dto
{
    public class ListarNotasFiscaisInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Filtro { get; set; }

        public long EmpresaId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataEmissao DESC";
            }
        }
    }
}
