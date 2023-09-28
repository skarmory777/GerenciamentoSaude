using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem
{
    [Table("SisDisparoDeMensagemItem")]
    public class DisparoDeMensagemItem : CamposPadraoCRUD
    {
        public string Origem { get; set; }

        public long? OrigemId { get; set; }

        public long? PessoaId { get; set; }

        public SisPessoa Pessoa { get; set; }

        public long? DisparoDeMensagemId { get; set; }

        public DisparoDeMensagem DisparoDeMensagem { get; set; }

        public long DisparoDeMensagemItemTipoId { get; set; }

        public DisparoDeMensagemItemTipo DisparoDeMensagemItemTipo { get; set; }
        [Index("Sis_Idx_DataProgramada")]
        public DateTime DataProgramada { get; set; }
        [Index("Sis_Idx_DataInicioDisparo")]
        public DateTime? DataInicioDisparo { get; set; }
        [Index("Sis_Idx_DataFinalDisparo")]
        public DateTime? DataFinalDisparo { get; set; }
        [Index("Sis_Idx_DataRecebimento")]
        public DateTime? DataRecebimento { get; set; }

        public string Mensagem { get; set; }

        public string Titulo { get; set; }

        public string Valor { get; set; }
    }


}
