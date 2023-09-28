using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    public class AtendimentoIndexDto : CamposPadraoCRUDDto
    {
        public string Unidade { get; set; }
        public string CodigoAtendimento { get; set; }
        public string Paciente { get; set; }

        public DateTime? PacienteNascimento { get; set; }

        public string PacienteIdade { get; set; }

        public string TipoLeito { get; set; }
        public string LeitoAtual { get; set; }
        public DateTime? DataRegistro { get; set; }
        public DateTime? DataInicioConta { get; set; }
        public DateTime? DataFim { get; set; }
        public string Convenio { get; set; }
        public string Matricula { get; set; }
        public string Guia { get; set; }
        public string NumeroGuia { get; set; }
        public string Empresa { get; set; }
        public DateTime? DataAlta { get; set; }
        public string Plano { get; set; }
        public string CodigoPaciente { get; set; }
        public bool IsControlaTev { get; set; }
        public string Medico { get; set; }
        public string Leito { get; set; }
        public long? AtendimentoMotivoCancelamentoId { get; set; }
        public string Classificacao { get; set; }
        public string CorClassificacao { get; set; }
        public string Protocolo { get; set; }
        public string Status { get; set; }

        public string CorFundo { get; set; }

        public string CorTexto { get; set; }

        public int? Senha { get; set; }

        public long? SenhaAtendimentoId { get; set; }

        public bool IsPendenteExames { get; set; }
        public bool IsPendenteMedicacao { get; set; }
        public bool IsPendenteProcedimento { get; set; }

        public int? StatusAguardando { get; set; }
        public int? StatusAtendido { get; set; }
        // public int? DiasAutorizados { get; set; }
        public DateTime? DataAutorizada { get; set; }
        public string CorStatusAutorizacao { get; set; }

        public long QtdLabResultadoExame { get; set; }
        public long QtdLauMovimentoItem { get; set; }
        public long QtdLabAssSolicitacaoExame { get; set; }

        public long QtdLauAssSolicitacaoExame { get; set; }
    }
}
