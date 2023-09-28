using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.SalasCirurgicas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoSalasCirurgicasDisponibilidades.Dto
{
    public class AgendamentoSalaCirurgicaDisponibilidadeDto : CamposPadraoCRUDDto
    {
        public AgendamentoCirurgicoDto AgendamentoCirurgico { get; set; }
        public long AgendamentoCirurgicoId { get; set; }
        public long SalaCirurgicaId { get; set; }
        public SalaCirurgicaDto SalaCirurgica { get; set; }
        public long TipoCirurgiaId { get; set; }
        public TipoCirurgiaDto TipoCirurgia { get; set; }
        public long IntervaloId { get; set; }
        public IntervaloDto Intervalo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFim { get; set; }
        public bool Domingo { get; set; }
        public bool Segunda { get; set; }
        public bool Terca { get; set; }
        public bool Quarta { get; set; }
        public bool Quinta { get; set; }
        public bool Sexta { get; set; }
        public bool Sabado { get; set; }
        public long? EmpresaId { get; set; }

        public EmpresaDto Empresa { get; set; }

        public static AgendamentoSalaCirurgicaDisponibilidadeDto Mapear(AgendamentoSalaCirurgicaDisponibilidade agendamentoSalaCirurgicaDisponibilidade)
        {
            AgendamentoSalaCirurgicaDisponibilidadeDto agendamentoSalaCirurgicaDisponibilidadeDto = new AgendamentoSalaCirurgicaDisponibilidadeDto();

            agendamentoSalaCirurgicaDisponibilidadeDto.Id = agendamentoSalaCirurgicaDisponibilidade.Id;
            agendamentoSalaCirurgicaDisponibilidadeDto.Codigo = agendamentoSalaCirurgicaDisponibilidade.Codigo;
            agendamentoSalaCirurgicaDisponibilidadeDto.Descricao = agendamentoSalaCirurgicaDisponibilidade.Descricao;
            // agendamentoSalaCirurgicaDisponibilidadeDto.AgendamentoCirurgicoId = agendamentoSalaCirurgicaDisponibilidade.AgendamentoCirurgicoId;
            agendamentoSalaCirurgicaDisponibilidadeDto.SalaCirurgicaId = agendamentoSalaCirurgicaDisponibilidade.SalaCirurgicaId;
            agendamentoSalaCirurgicaDisponibilidadeDto.TipoCirurgiaId = agendamentoSalaCirurgicaDisponibilidade.TipoCirurgiaId;
            agendamentoSalaCirurgicaDisponibilidadeDto.DataInicio = agendamentoSalaCirurgicaDisponibilidade.DataInicio;
            agendamentoSalaCirurgicaDisponibilidadeDto.DataFim = agendamentoSalaCirurgicaDisponibilidade.DataFim;
            agendamentoSalaCirurgicaDisponibilidadeDto.HoraInicio = agendamentoSalaCirurgicaDisponibilidade.HoraInicio;
            agendamentoSalaCirurgicaDisponibilidadeDto.HoraFim = agendamentoSalaCirurgicaDisponibilidade.HoraFim;
            agendamentoSalaCirurgicaDisponibilidadeDto.Domingo = agendamentoSalaCirurgicaDisponibilidade.Domingo;
            agendamentoSalaCirurgicaDisponibilidadeDto.Segunda = agendamentoSalaCirurgicaDisponibilidade.Segunda;
            agendamentoSalaCirurgicaDisponibilidadeDto.Terca = agendamentoSalaCirurgicaDisponibilidade.Terca;
            agendamentoSalaCirurgicaDisponibilidadeDto.Quarta = agendamentoSalaCirurgicaDisponibilidade.Quarta;
            agendamentoSalaCirurgicaDisponibilidadeDto.Quinta = agendamentoSalaCirurgicaDisponibilidade.Quinta;
            agendamentoSalaCirurgicaDisponibilidadeDto.Sexta = agendamentoSalaCirurgicaDisponibilidade.Sexta;
            agendamentoSalaCirurgicaDisponibilidadeDto.Sabado = agendamentoSalaCirurgicaDisponibilidade.Sabado;
            agendamentoSalaCirurgicaDisponibilidadeDto.EmpresaId = agendamentoSalaCirurgicaDisponibilidade.EmpresaId;


            //if(agendamentoSalaCirurgicaDisponibilidade.AgendamentoCirurgico !=null)
            //{
            //    agendamentoSalaCirurgicaDisponibilidadeDto.AgendamentoCirurgico = AgendamentoCirurgicoDto.Mapear(agendamentoSalaCirurgicaDisponibilidade.AgendamentoCirurgico);
            //}

            if (agendamentoSalaCirurgicaDisponibilidade.SalaCirurgica != null)
            {
                agendamentoSalaCirurgicaDisponibilidadeDto.SalaCirurgica = SalaCirurgicaDto.Mapear(agendamentoSalaCirurgicaDisponibilidade.SalaCirurgica);
            }

            if (agendamentoSalaCirurgicaDisponibilidade.TipoCirurgia != null)
            {
                agendamentoSalaCirurgicaDisponibilidadeDto.TipoCirurgia = TipoCirurgiaDto.Mapear(agendamentoSalaCirurgicaDisponibilidade.TipoCirurgia);
            }

            if (agendamentoSalaCirurgicaDisponibilidade.Intervalo != null)
            {
                agendamentoSalaCirurgicaDisponibilidadeDto.Intervalo = IntervaloDto.Mapear(agendamentoSalaCirurgicaDisponibilidade.Intervalo);
            }

            if (agendamentoSalaCirurgicaDisponibilidade.Empresa != null)
            {
                agendamentoSalaCirurgicaDisponibilidadeDto.Empresa = EmpresaDto.Mapear(agendamentoSalaCirurgicaDisponibilidade.Empresa);
            }


            return agendamentoSalaCirurgicaDisponibilidadeDto;
        }


        public static List<AgendamentoSalaCirurgicaDisponibilidadeDto> Mapear(List<AgendamentoSalaCirurgicaDisponibilidade> disponibilidades)
        {
            List<AgendamentoSalaCirurgicaDisponibilidadeDto> disponibilidadesDto = new List<AgendamentoSalaCirurgicaDisponibilidadeDto>();

            if (disponibilidades != null)
            {
                foreach (var item in disponibilidades)
                {
                    disponibilidadesDto.Add(Mapear(item));
                }
            }

            return disponibilidadesDto;
        }

    }
}
