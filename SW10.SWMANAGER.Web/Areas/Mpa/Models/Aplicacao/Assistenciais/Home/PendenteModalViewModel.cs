using System;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Home
{
    public class PendenteModalViewModel
    {
        public long? AtendimentoId { get; set; }

        public DateTime Data { get; set; }

        public bool IsPendenteExames { get; set; }

        public bool IsPendenteMedicacao { get; set; }

        public bool IsPendenteProcedimento { get; set; }
    }
}