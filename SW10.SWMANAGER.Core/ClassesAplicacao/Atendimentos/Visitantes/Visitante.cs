using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Visitantes
{
    [Table("AteVisitante")]
    public class Visitante : CamposPadraoCRUD
    {
        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(14)]
        public string Documento { get; set; }

        public bool IsAcompanhante { get; set; }

        public bool IsVisitante { get; set; }

        public bool IsMedico { get; set; }

        public bool IsEmergencia { get; set; }

        public bool IsInternado { get; set; }

        public bool IsFornecedor { get; set; }

        public bool IsSetor { get; set; }

        public byte[] Foto { get; set; }

        public string FotoMimeType { get; set; }

        [Index("Ate_Idx_DataEntrada")]
        [DataType(DataType.DateTime)]
        public DateTime? DataEntrada { get; set; }

        [Index("Ate_Idx_DataSaida")]
        [DataType(DataType.DateTime)]
        public DateTime? DataSaida { get; set; }


        [ForeignKey("UnidadeOrganizacional")]
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }


        [ForeignKey("Leito")]
        public long? LeitoId { get; set; }
        public Leito Leito { get; set; }


        [ForeignKey("Atendimento")]
        public long? AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; }

        [ForeignKey("Fornecedor")]
        public long? FornecedorId { get; set; }
        public SisFornecedor Fornecedor { get; set; }
    }
}
