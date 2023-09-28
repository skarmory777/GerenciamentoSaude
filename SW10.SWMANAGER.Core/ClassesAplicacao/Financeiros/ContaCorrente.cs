using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinContaCorrente")]
    public class ContaCorrente : CamposPadraoCRUD
    {
        public long TipoContaCorrenteId { get; set; }

        [ForeignKey("TipoContaCorrenteId")]
        public TipoContaCorrente TipoContaCorrente { get; set; }

        public long AgenciaId { get; set; }

        [ForeignKey("AgenciaId")]
        public Agencia Agencia { get; set; }

        public long EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
        [Index("Fin_Idx_DataAbertura")]
        public DateTime DataAbertura { get; set; }
        public string NomeGerente { get; set; }
        public decimal? LimiteCredito { get; set; }
        public string Observacao { get; set; }
        public bool IsContaNaoOperacional { get; set; }
    }
}
