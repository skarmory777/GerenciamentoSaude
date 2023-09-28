using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas
{
    [Table("FatContaKit")]
    public class FaturamentoContaKit : CamposPadraoCRUD
    {
        [ForeignKey("FaturamentoConta"), Column("FatContaId")]
        public long? FaturamentoContaId { get; set; }
        public FaturamentoConta FaturamentoConta { get; set; }

        [Index("Fat_Idx_Data")]
        [DataType(DataType.DateTime)]
        public DateTime? Data { get; set; }

        public float Qtde { get; set; }

        //[ForeignKey("LocalUtilizacao"), Column("LocalUtilizacaoId")]
        //public long? LocalUtilizacaoId { get; set; }
        //public LocalUtilizacao LocalUtilizacao { get; set; }

        [ForeignKey("Terceirizado"), Column("TerceirizadoId")]
        public long? TerceirizadoId { get; set; }
        public Terceirizado Terceirizado { get; set; }

        [ForeignKey("CentroCusto"), Column("CentroCustoId")]
        public long? CentroCustoId { get; set; }
        public CentroCusto CentroCusto { get; set; }

        [ForeignKey("Turno"), Column("TurnoId")]
        public long? TurnoId { get; set; }
        public Turno Turno { get; set; }

        //[ForeignKey("TipoLeito"), Column("TipoLeitoId")]
        //public long? TipoLeitoId { get; set; }
        //public TipoLeito TipoLeito { get; set; }


        [ForeignKey("TipoAcomodacao"), Column("TipoAcomodacaoId")]
        public long? TipoAcomodacaoId { get; set; }
        public TipoAcomodacao TipoAcomodacao { get; set; }

        [Index("Fat_Idx_HoraIncio")]
        [DataType(DataType.DateTime)]
        public DateTime? HoraIncio { get; set; }
        [Index("Fat_Idx_HoraFim")]
        public DateTime? HoraFim { get; set; }

        [ForeignKey("Medico"), Column("MedicoId")]
        public long? MedicoId { get; set; }
        public Medico Medico { get; set; }

        [ForeignKey("FaturamentoKit"), Column("FatKitId")]
        public long? FaturamentoKitId { get; set; }
        public FaturamentoKit FaturamentoKit { get; set; }
    }

}


