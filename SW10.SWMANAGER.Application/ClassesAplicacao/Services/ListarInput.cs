using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    public class ListarInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime StartDateNotNull { get; set; }

        public DateTime EndDateNotNull { get; set; }

        public long? EmpresaId { get; set; }

        public string Id { get; set; }

        public string PrincipalId { get; set; }

        public long? OperacaoId { get; set; }

        public virtual void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime desc";
            }
        }
    }
}
