using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    public class ListarQuitacao
    {
        public long? Id { get; set; }

        public bool IsCredito { get; set; }

        public DateTimeOffset? DataMovimento { get; set; }

        public string PessoaNome { get; set; }

        public string MeioPagamentoDescricao { get; set; }

        public decimal ValorTotal { get; set; }

        public DateTimeOffset? DataCompensado { get; set; }

        public DateTimeOffset? DataConsolidado { get; set; }

        public string Observacao { get; set; }
        
        public string ContaCorrenteDescricao { get; set; }
    }
}


