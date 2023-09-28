using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas
{
    [Table("AteAgendamentoItemFaturamento")]
    public class AgendamentoItemFaturamento : CamposPadraoCRUD
    {
        public long? AgendamentoCirurgicoId { get; set; }

        public long? FaturamentoItemId { get; set; }

        [ForeignKey("FaturamentoItemId")]
        public FaturamentoItem FaturamentoItem { get; set; }

        [ForeignKey("AgendamentoCirurgicoId")]
        public AgendamentoCirurgico AgendamentoCirurgico { get; set; }

        public decimal Quantidade { get; set; }

        [Index("Ate_Idx_IsCirurgica")]
        public bool IsCirurgica { get; set; }
        public decimal? ValorDesconto { get; set; }

        public long? UsuarioDescontoId { get; set; }

        [ForeignKey("UsuarioDescontoId")]
        public User UsuarioDesconto { get; set; }
    }
}
