using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class ListarSolicitacoesExamesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long PacienteId { get; set; }

        public long EmpresaId { get; set; }

        public long UnidadeOrganizacionalId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Filtro { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataSolicitacao desc";
            }
        }
    }
}
