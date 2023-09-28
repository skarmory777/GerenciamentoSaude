using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto
{
    public class ListarAutorizacaoProcedimentoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public long? ConvenioId { get; set; }
        public long? FaturamentoItemId { get; set; }
        public DateTime? PeridoDe { get; set; }
        public DateTime? PeridoAte { get; set; }


        public void Normalize()
        {
            Sorting = "Id";
        }
    }
}
