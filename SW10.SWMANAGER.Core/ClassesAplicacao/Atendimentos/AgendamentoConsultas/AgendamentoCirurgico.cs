using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas
{
    [Table("AteAgendamentoCirurgico")]
    public class AgendamentoCirurgico : CamposPadraoCRUD
    {
        public long AgendamentoConsultaId { get; set; }

        public long? AgendamentoSalaCirurgicaDisponibilidadeId { get; set; }
        public bool IsEletiva { get; set; }
        public bool IsEmergencia { get; set; }
        public bool IsPossuiOPME { get; set; }
        public long StatusAutorizacaoCirurgiaExame { get; set; }
        public long StatusAutorizacaoOPME { get; set; }
        public bool IsNecessitaSangue { get; set; }
        public bool IsNecessitaVideo { get; set; }
        public bool IsNecessitaCTI { get; set; }
        public bool IsNecesssitaItencificador { get; set; }
        public string OPMESolicitada { get; set; }
        public string OPMEAutorizada { get; set; }
        public bool IsPossuiAlergia { get; set; }
        public string Alergias { get; set; }
        public bool IsPossuiPrecaucoes { get; set; }
        public string Precaucoes { get; set; }


        [ForeignKey("AgendamentoSalaCirurgicaDisponibilidadeId")]
        public AgendamentoSalaCirurgicaDisponibilidade AgendamentoSalaCirurgicaDisponibilidade { get; set; }

        [ForeignKey("AgendamentoConsultaId")]
        public AgendamentoConsulta AgendamentoConsulta { get; set; }

        public List<AgendamentoItemFaturamento> Cirurgias { get; set; }
        public List<AgendamentoMaterial> MateriaisOPME { get; set; }
    }
}
