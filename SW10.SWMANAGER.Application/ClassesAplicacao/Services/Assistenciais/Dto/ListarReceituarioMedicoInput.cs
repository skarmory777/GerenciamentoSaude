using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class ListarReceituarioMedicoInput : ListarInput
    {
        public long? AtendimentoId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public void Normalize()
        {
            Sorting = "Descricao";
        }
    }
}
