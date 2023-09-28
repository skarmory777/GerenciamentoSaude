using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstMovimentoBaixa")]
    public class EstMovimentoBaixa : CamposPadraoCRUD
    {
        public long MovimentoBaixaPrincipalId { get; set; }
        public long MovimentoBaixaId { get; set; }

        [ForeignKey("MovimentoBaixaPrincipalId")]
        public EstoqueMovimento MovimentoBaixaPrincipal { get; set; }

        [ForeignKey("MovimentoBaixaId")]
        public EstoqueMovimento MovimentoBaixa { get; set; }

    }
}
