using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem
{
    [Table("SisDisparoDeMensagem")]
    public class DisparoDeMensagem : CamposPadraoCRUD
    {
        [Index("Sis_Idx_DataProgramada")]
        public DateTime DataProgramada { get; set; }
        [Index("Sis_Idx_DataInicioDisparo")]
        public DateTime? DataInicioDisparo { get; set; }
        [Index("Sis_Idx_DataFinalDisparo")]
        public DateTime? DataFinalDisparo { get; set; }
        [Index("Sis_Idx_DisparoAtivo")]
        public bool DisparoAtivo { get; set; }

        public string Mensagem { get; set; }

        public string Titulo { get; set; }

        public long Total { get; set; }

        public long TotalEnviado { get; set; }

        public long TotalRecebido { get; set; }

        public ICollection<DisparoDeMensagemItem> DisparoDeMensagemItems { get; set; }
    }
}
