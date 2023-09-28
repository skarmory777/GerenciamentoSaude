using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Dto
{
    public class ListarRegistroInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long? AtendimentoId { get; set; }
        
        public long? OperacaoId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public void Normalize()
        {
            Sorting = "Descricao";
        }
    }
}
