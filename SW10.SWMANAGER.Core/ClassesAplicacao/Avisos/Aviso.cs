using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace SW10.SWMANAGER.ClassesAplicacao.Avisos
{
    [Table("SisAvisos")]
    public class Aviso : CamposPadraoCRUD
    {
        public string Titulo { get; set; }
        
        public string Mensagem { get; set; }
        [Index("Sis_Idx_DataProgramada")]
        public DateTime? DataProgramada { get; set; }

        [Index("Sis_Idx_DataInicioDisparo")]
        public DateTime? DataInicioDisparo { get; set; }

        [Index("Sis_Idx_DataFinalDisparo")]
        public DateTime? DataFinalDisparo { get; set; }
        
        public bool Bloquear { get; set; }
        
        public bool DisparoAtivo { get; set; }
        
        public long TotalEnviado { get; set; }
        
        public ICollection<AvisoGrupo> Grupos { get; set; }

    }
}