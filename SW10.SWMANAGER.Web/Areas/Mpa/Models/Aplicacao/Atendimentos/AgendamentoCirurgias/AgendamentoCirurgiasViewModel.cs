using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoCirurgias
{
    public class AgendamentoCirurgiasViewModel : AgendamentoCirurgicoDto
    {

        public bool IsEditMode { get; set; }

        public AgendamentoCirurgiasViewModel() { }


        public AgendamentoCirurgiasViewModel(AgendamentoCirurgicoDto agendamentoCirurgicoDto)
        {
            this.Id = agendamentoCirurgicoDto.AgendamentoConsultaId;
            this.Codigo = agendamentoCirurgicoDto.Codigo;
            this.Descricao = agendamentoCirurgicoDto.Descricao;
            //   this.AgendamentoConsultaId = agendamentoCirurgicoDto.AgendamentoConsultaId;
            this.IsEletiva = agendamentoCirurgicoDto.IsEletiva;
            this.IsEmergencia = agendamentoCirurgicoDto.IsEmergencia;
            this.IsPossuiOPME = agendamentoCirurgicoDto.IsPossuiOPME;
            this.StatusAutorizacaoCirurgiaExame = agendamentoCirurgicoDto.StatusAutorizacaoCirurgiaExame;
            this.StatusAutorizacaoOPME = agendamentoCirurgicoDto.StatusAutorizacaoOPME;
            this.IsNecessitaSangue = agendamentoCirurgicoDto.IsNecessitaSangue;
            this.IsNecessitaVideo = agendamentoCirurgicoDto.IsNecessitaVideo;
            this.IsNecessitaCTI = agendamentoCirurgicoDto.IsNecessitaCTI;
            this.IsNecesssitaItencificador = agendamentoCirurgicoDto.IsNecesssitaItencificador;
            this.OPMESolicitada = agendamentoCirurgicoDto.OPMESolicitada;
            this.OPMEAutorizada = agendamentoCirurgicoDto.OPMEAutorizada;
            this.IsPossuiAlergia = agendamentoCirurgicoDto.IsPossuiAlergia;
            this.Alergias = agendamentoCirurgicoDto.Alergias;
            this.IsPossuiPrecaucoes = agendamentoCirurgicoDto.IsPossuiPrecaucoes;
            this.Precaucoes = agendamentoCirurgicoDto.Precaucoes;
            //this.AgendamentoSalaCirurgicaDisponibilidadeId = agendamentoCirurgicoDto.AgendamentoConsultaMedicoDisponibilidadeId;

            this.ConvenioId = agendamentoCirurgicoDto.ConvenioId;
            this.DataAgendamento = agendamentoCirurgicoDto.DataAgendamento;
            this.Descricao = agendamentoCirurgicoDto.Descricao;
            this.HoraAgendamento = agendamentoCirurgicoDto.HoraAgendamento;
            this.Notas = agendamentoCirurgicoDto.Notas;
            this.PacienteId = agendamentoCirurgicoDto.PacienteId;
            this.PlanoId = agendamentoCirurgicoDto.PlanoId;
            this.QuantidadeHorarios = agendamentoCirurgicoDto.QuantidadeHorarios;
            this.Convenio = agendamentoCirurgicoDto.Convenio;
            this.Plano = agendamentoCirurgicoDto.Plano;
            this.Paciente = agendamentoCirurgicoDto.Paciente;

            this.AgendamentoSalaCirurgicaDisponibilidade = agendamentoCirurgicoDto.AgendamentoSalaCirurgicaDisponibilidade;
            this.Cirurgias = agendamentoCirurgicoDto.Cirurgias;

            this.MedicoId = agendamentoCirurgicoDto.MedicoId;
            this.Medico = agendamentoCirurgicoDto.Medico;
            this.MedicoEspecialidadeId = agendamentoCirurgicoDto.MedicoEspecialidadeId;
            this.MedicoEspecialidade = agendamentoCirurgicoDto.MedicoEspecialidade;


            this.NomeReservante = agendamentoCirurgicoDto.NomeReservante;
            this.TelefoneReservante = agendamentoCirurgicoDto.TelefoneReservante;
            this.DataNascimentoReservante = agendamentoCirurgicoDto.DataNascimentoReservante;
            this.CPF = agendamentoCirurgicoDto.CPF;
            this.MedicoObsrvacao = agendamentoCirurgicoDto.Medico?.SisPessoa?.Observacao;

            this.StatusId = agendamentoCirurgicoDto.StatusId;
            this.AgendamentoStatus = agendamentoCirurgicoDto.AgendamentoStatus;
            this.Sexo = agendamentoCirurgicoDto.Sexo;

        }
    }
}