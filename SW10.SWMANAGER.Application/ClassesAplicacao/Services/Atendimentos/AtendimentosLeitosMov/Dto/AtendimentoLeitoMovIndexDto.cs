using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentosLeitosMov.Dto
{
    public class AtendimentoLeitoMovIndexDto : CamposPadraoCRUDDto
    {
        public string CodigoAtendimento { get; set; }
        public string Leito { get; set; }
        public string TipoLeito { get; set; }
        public string Paciente { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlta { get; set; }
        public bool IsUltimoHistorico { get; set; }
    }
}
