using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [Table("ControleProducao")]
    public class ControleProducao : CamposPadraoCRUD
    {
        [StringLength(500)]
        public string Observacao { get; set; }

        public int Pontuacao { get; set; }

        public int Status { get; set; }

        [Index("Idx_DataInicial")]
        [DataType(DataType.DateTime)]
        public DateTime DataInicial { get; set; }

        [Index("Idx_DataFinal")]
        [DataType(DataType.DateTime)]
        public DateTime DataFinal { get; set; }

        [Index("Idx_DataAprovacao")]
        [DataType(DataType.DateTime)]
        public DateTime DataAprovacao { get; set; }

        public long DesenvolvedorId { get; set; }

        public long UsuarioAprovacaoId { get; set; }

        public long TabelaSistemaId { get; set; }

        [ForeignKey("DesenvolvedorId")]
        public User Desenvolvedor { get; set; }

        [ForeignKey("UsuarioAprovacaoId")]
        public User UsuarioAprovacao { get; set; }

        [ForeignKey("TabelaSistemaId")]
        public ConsultorTabela TabelaSistema { get; set; }
    }
}
