using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnosticos.Laudos.Dto
{
    public class ListarLauMovimentoItensInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public long? ConvenioId { get; set; }
        public long? PacienteId { get; set; }
        public DateTime? EmissaoDe { get; set; }
        public DateTime? EmissaoAte { get; set; }
        public string HorarioInicial { get; set; }
        public string HorarioFinal { get; set; }
        public long? ModalidadeId { get; set; }
        public long? AtendimentoId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
