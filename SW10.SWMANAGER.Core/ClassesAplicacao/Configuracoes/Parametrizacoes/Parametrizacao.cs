using System;
using SW10.SWMANAGER.ClassesAplicacao;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Parametrizacoes
{
    [Table("SisParametrizacoes")]
    public class Parametrizacao : CamposPadraoCRUD
    {
        public bool IsHabilitaControleDeIp { get; set; }

        [Index("Sis_Idx_SolicitacaoExameHoraOutroDia")]
        public TimeSpan? SolicitacaoExameHoraOutroDia { get; set; }

        [Index("Sis_Idx_PrescricaoMedicaHoraOutroDia")]
        public TimeSpan? PrescricaoMedicaHoraOutroDia { get; set; }
        
        public bool IsHabilitaAssistencialColetaAutomatica { get; set; }
    }
}
