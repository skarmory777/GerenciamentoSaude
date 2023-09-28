using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoSalasCirurgicasDisponibilidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas
{
    public class AgendamentoCirurgicoDto : AgendamentoConsultaDto
    {
        public long AgendamentoConsultaId { get; set; }

        //  public long? SalaCirurgicaId { get; set; }
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
        public int QuantidadeHorarios { get; set; }
        // public long? AgendamentoSalaCirurgicaDisponibilidadeId { get; set; }
        public AgendamentoSalaCirurgicaDisponibilidadeDto AgendamentoSalaCirurgicaDisponibilidade { get; set; }

        public string CirurgiasJson { get; set; }

        public List<AgendamentoItemFaturamentoDto> Cirurgias { get; set; }

        public string MateriaisOPMEJson { get; set; }

        public List<AgendamentoMaterialDto> MateriaisOPME { get; set; }


        public string MateriaisJson { get; set; }

        public string CPF { get; set; }
        public string MedicoObsrvacao { get; set; }
        public string Sexo { get; set; }


        // public SalaCirurgicaDto SalaCirurgica { get; set; }

        public static AgendamentoCirurgicoDto Mapear(AgendamentoCirurgico agendamentoCirurgico)
        {
            AgendamentoCirurgicoDto agendamentoCirurgicoDto = new AgendamentoCirurgicoDto();

            agendamentoCirurgicoDto.Id = agendamentoCirurgico.Id;
            agendamentoCirurgicoDto.Codigo = agendamentoCirurgico.Codigo;
            agendamentoCirurgicoDto.Descricao = agendamentoCirurgico.Descricao;
            agendamentoCirurgicoDto.AgendamentoConsultaId = agendamentoCirurgico.AgendamentoConsultaId;
            agendamentoCirurgicoDto.IsEletiva = agendamentoCirurgico.IsEletiva;
            agendamentoCirurgicoDto.IsEmergencia = agendamentoCirurgico.IsEmergencia;
            agendamentoCirurgicoDto.IsPossuiOPME = agendamentoCirurgico.IsPossuiOPME;
            agendamentoCirurgicoDto.StatusAutorizacaoCirurgiaExame = agendamentoCirurgico.StatusAutorizacaoCirurgiaExame;
            agendamentoCirurgicoDto.StatusAutorizacaoOPME = agendamentoCirurgico.StatusAutorizacaoOPME;
            agendamentoCirurgicoDto.IsNecessitaSangue = agendamentoCirurgico.IsNecessitaSangue;
            agendamentoCirurgicoDto.IsNecessitaVideo = agendamentoCirurgico.IsNecessitaVideo;
            agendamentoCirurgicoDto.IsNecessitaCTI = agendamentoCirurgico.IsNecessitaCTI;
            agendamentoCirurgicoDto.IsNecesssitaItencificador = agendamentoCirurgico.IsNecesssitaItencificador;
            agendamentoCirurgicoDto.OPMESolicitada = agendamentoCirurgico.OPMESolicitada;
            agendamentoCirurgicoDto.OPMEAutorizada = agendamentoCirurgico.OPMEAutorizada;
            agendamentoCirurgicoDto.IsPossuiAlergia = agendamentoCirurgico.IsPossuiAlergia;
            agendamentoCirurgicoDto.Alergias = agendamentoCirurgico.Alergias;
            agendamentoCirurgicoDto.IsPossuiPrecaucoes = agendamentoCirurgico.IsPossuiPrecaucoes;
            agendamentoCirurgicoDto.Precaucoes = agendamentoCirurgico.Precaucoes;


            if (agendamentoCirurgico.AgendamentoConsulta != null)
            {
                agendamentoCirurgicoDto.ConvenioId = agendamentoCirurgico.AgendamentoConsulta.ConvenioId;
                agendamentoCirurgicoDto.DataAgendamento = agendamentoCirurgico.AgendamentoConsulta.DataAgendamento;
                agendamentoCirurgicoDto.Descricao = agendamentoCirurgico.AgendamentoConsulta.Descricao;
                agendamentoCirurgicoDto.HoraAgendamento = agendamentoCirurgico.AgendamentoConsulta.HoraAgendamento;
                agendamentoCirurgicoDto.Notas = agendamentoCirurgico.AgendamentoConsulta.Notas;
                agendamentoCirurgicoDto.PacienteId = agendamentoCirurgico.AgendamentoConsulta.PacienteId;
                agendamentoCirurgicoDto.PlanoId = agendamentoCirurgico.AgendamentoConsulta.PlanoId;
                agendamentoCirurgicoDto.QuantidadeHorarios = agendamentoCirurgico.AgendamentoConsulta.QuantidadeHorarios;

                if (agendamentoCirurgico.AgendamentoConsulta.Paciente != null)
                {
                    agendamentoCirurgicoDto.NomeReservante = agendamentoCirurgico.AgendamentoConsulta.Paciente.NomeCompleto;
                    agendamentoCirurgicoDto.DataNascimentoReservante = agendamentoCirurgico.AgendamentoConsulta.Paciente.Nascimento;
                    agendamentoCirurgicoDto.TelefoneReservante = agendamentoCirurgico.AgendamentoConsulta.Paciente.Telefone1;
                    agendamentoCirurgicoDto.CPF = agendamentoCirurgico.AgendamentoConsulta.Paciente.Cpf;
                    agendamentoCirurgicoDto.Sexo = agendamentoCirurgico.AgendamentoConsulta.Paciente.Sexo?.Descricao;
                }
                else
                {
                    agendamentoCirurgicoDto.NomeReservante = agendamentoCirurgico.AgendamentoConsulta.NomeReservante;
                    agendamentoCirurgicoDto.DataNascimentoReservante = agendamentoCirurgico.AgendamentoConsulta.DataNascimentoReservante;
                    agendamentoCirurgicoDto.TelefoneReservante = agendamentoCirurgico.AgendamentoConsulta.TelefoneReservante;
                    agendamentoCirurgicoDto.CPF = agendamentoCirurgico.AgendamentoConsulta.CPF;
                }

                if (agendamentoCirurgico.AgendamentoConsulta.Convenio != null)
                {
                    agendamentoCirurgicoDto.Convenio = ConvenioDto.Mapear(agendamentoCirurgico.AgendamentoConsulta.Convenio);
                }

                if (agendamentoCirurgico.AgendamentoConsulta.Plano != null)
                {
                    agendamentoCirurgicoDto.Plano = PlanoDto.Mapear(agendamentoCirurgico.AgendamentoConsulta.Plano);
                }

                if (agendamentoCirurgico.AgendamentoConsulta.Paciente != null)
                {
                    agendamentoCirurgicoDto.Paciente = PacienteDto.Mapear(agendamentoCirurgico.AgendamentoConsulta.Paciente);
                }

                agendamentoCirurgicoDto.MedicoId = agendamentoCirurgico.AgendamentoConsulta.MedicoId ?? 0;
                if (agendamentoCirurgico.AgendamentoConsulta.Medico != null)
                {
                    agendamentoCirurgicoDto.Medico = MedicoDto.Mapear(agendamentoCirurgico.AgendamentoConsulta.Medico);
                }

                agendamentoCirurgicoDto.MedicoEspecialidadeId = agendamentoCirurgico.AgendamentoConsulta.MedicoEspecialidadeId ?? 0;

                if (agendamentoCirurgico.AgendamentoConsulta.MedicoEspecialidade != null)
                {
                    agendamentoCirurgicoDto.MedicoEspecialidade = MedicoEspecialidadeDto.Mapear(agendamentoCirurgico.AgendamentoConsulta.MedicoEspecialidade);
                }

                agendamentoCirurgicoDto.StatusId = agendamentoCirurgico.AgendamentoConsulta.StatusId;

                if (agendamentoCirurgico.AgendamentoConsulta.AgendamentoStatus != null)
                {
                    agendamentoCirurgicoDto.AgendamentoStatus = AgendamentoStatusDto.Mapear(agendamentoCirurgico.AgendamentoConsulta.AgendamentoStatus);
                }

            }


            if (agendamentoCirurgico.AgendamentoSalaCirurgicaDisponibilidade != null)
            {
                agendamentoCirurgicoDto.AgendamentoSalaCirurgicaDisponibilidade = AgendamentoSalaCirurgicaDisponibilidadeDto.Mapear(agendamentoCirurgico.AgendamentoSalaCirurgicaDisponibilidade);
            }

            agendamentoCirurgicoDto.Cirurgias = AgendamentoItemFaturamentoDto.Mapear(agendamentoCirurgico.Cirurgias);

            agendamentoCirurgicoDto.MateriaisOPME = AgendamentoMaterialDto.Mapear(agendamentoCirurgico.MateriaisOPME);

            return agendamentoCirurgicoDto;
        }

        public static List<AgendamentoCirurgicoDto> Mapear(List<AgendamentoCirurgico> agendamentos)
        {
            List<AgendamentoCirurgicoDto> agendamentosDto = new List<AgendamentoCirurgicoDto>();

            if (agendamentos != null)
            {
                foreach (var item in agendamentos)
                {
                    agendamentosDto.Add(AgendamentoCirurgicoDto.Mapear(item));
                }
            }

            return agendamentosDto;
        }



        public static AgendamentoCirurgico Mapear(AgendamentoCirurgicoDto agendamentoCirurgicoDto)
        {
            AgendamentoCirurgico agendamentoCirurgico = new AgendamentoCirurgico();

            agendamentoCirurgico.Id = agendamentoCirurgicoDto.Id;
            agendamentoCirurgico.Codigo = agendamentoCirurgicoDto.Codigo;
            agendamentoCirurgico.Descricao = agendamentoCirurgicoDto.Descricao;
            agendamentoCirurgico.AgendamentoConsultaId = agendamentoCirurgicoDto.AgendamentoConsultaId;
            agendamentoCirurgico.IsEletiva = agendamentoCirurgicoDto.IsEletiva;
            agendamentoCirurgico.IsEmergencia = agendamentoCirurgicoDto.IsEmergencia;
            agendamentoCirurgico.IsPossuiOPME = agendamentoCirurgicoDto.IsPossuiOPME;
            agendamentoCirurgico.StatusAutorizacaoCirurgiaExame = agendamentoCirurgicoDto.StatusAutorizacaoCirurgiaExame;
            agendamentoCirurgico.StatusAutorizacaoOPME = agendamentoCirurgicoDto.StatusAutorizacaoOPME;
            agendamentoCirurgico.IsNecessitaSangue = agendamentoCirurgicoDto.IsNecessitaSangue;
            agendamentoCirurgico.IsNecessitaVideo = agendamentoCirurgicoDto.IsNecessitaVideo;
            agendamentoCirurgico.IsNecessitaCTI = agendamentoCirurgicoDto.IsNecessitaCTI;
            agendamentoCirurgico.IsNecesssitaItencificador = agendamentoCirurgicoDto.IsNecesssitaItencificador;
            agendamentoCirurgico.OPMESolicitada = agendamentoCirurgicoDto.OPMESolicitada;
            agendamentoCirurgico.OPMEAutorizada = agendamentoCirurgicoDto.OPMEAutorizada;
            agendamentoCirurgico.IsPossuiAlergia = agendamentoCirurgicoDto.IsPossuiAlergia;
            agendamentoCirurgico.Alergias = agendamentoCirurgicoDto.Alergias;
            agendamentoCirurgico.IsPossuiPrecaucoes = agendamentoCirurgicoDto.IsPossuiPrecaucoes;
            agendamentoCirurgico.Precaucoes = agendamentoCirurgicoDto.Precaucoes;
            agendamentoCirurgico.AgendamentoSalaCirurgicaDisponibilidadeId = agendamentoCirurgicoDto.AgendamentoConsultaMedicoDisponibilidadeId;

            agendamentoCirurgico.AgendamentoConsulta = new AgendamentoConsulta();

            agendamentoCirurgico.AgendamentoConsulta.MedicoId = agendamentoCirurgicoDto.MedicoId;
            agendamentoCirurgico.AgendamentoConsulta.MedicoEspecialidadeId = agendamentoCirurgicoDto.MedicoEspecialidadeId;
            agendamentoCirurgico.AgendamentoConsulta.ConvenioId = agendamentoCirurgicoDto.ConvenioId;
            agendamentoCirurgico.AgendamentoConsulta.DataAgendamento = agendamentoCirurgicoDto.DataAgendamento;
            agendamentoCirurgico.AgendamentoConsulta.Descricao = agendamentoCirurgicoDto.Descricao;
            agendamentoCirurgico.AgendamentoConsulta.HoraAgendamento = agendamentoCirurgicoDto.HoraAgendamento;
            agendamentoCirurgico.AgendamentoConsulta.Notas = agendamentoCirurgicoDto.Notas;
            agendamentoCirurgico.AgendamentoConsulta.PacienteId = agendamentoCirurgicoDto.PacienteId;
            agendamentoCirurgico.AgendamentoConsulta.PlanoId = agendamentoCirurgicoDto.PlanoId;
            agendamentoCirurgico.AgendamentoConsulta.QuantidadeHorarios = agendamentoCirurgicoDto.QuantidadeHorarios;

            agendamentoCirurgico.AgendamentoConsulta.NomeReservante = agendamentoCirurgicoDto.NomeReservante;
            agendamentoCirurgico.AgendamentoConsulta.TelefoneReservante = agendamentoCirurgicoDto.TelefoneReservante;
            agendamentoCirurgico.AgendamentoConsulta.DataNascimentoReservante = agendamentoCirurgicoDto.DataNascimentoReservante;
            agendamentoCirurgico.AgendamentoConsulta.CPF = agendamentoCirurgicoDto.CPF;
            agendamentoCirurgico.AgendamentoConsulta.StatusId = agendamentoCirurgicoDto.StatusId;

            //if (agendamentoCirurgicoDto.AgendamentoSalaCirurgicaDisponibilidade != null)
            //{
            //    agendamentoCirurgico.AgendamentoSalaCirurgicaDisponibilidade = AgendamentoSalaCirurgicaDisponibilidadeDto.Mapear(agendamentoCirurgico.AgendamentoSalaCirurgicaDisponibilidade);
            //}

            return agendamentoCirurgico;
        }

    }
}
