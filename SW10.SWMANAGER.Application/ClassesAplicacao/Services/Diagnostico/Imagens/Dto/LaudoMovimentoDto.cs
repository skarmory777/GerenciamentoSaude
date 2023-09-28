using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    [AutoMap(typeof(LaudoMovimento))]
    public class LaudoMovimentoDto : CamposPadraoCRUDDto
    {
        public long AtendimentoId { get; set; }
        public long LaudoMovimentoStatusId { get; set; }
        public long? ConvenioId { get; set; }
        public long? LeitoId { get; set; }
        public bool IsContraste { get; set; }
        public string QtdeConstraste { get; set; }
        public string Obs { get; set; }
        public AtendimentoDto Atendimento { get; set; }
        public LaudoMovimentoStatusDto LaudoMovimentoStatus { get; set; }
        public ConvenioDto Convenio { get; set; }
        public LeitoDto Leito { get; set; }
        public long AmbulatorioInternacao { get; set; }
        public DateTime? DataRegistro { get; set; }
        public DateTime? HoraRegistro { get; set; }
        public long? CentroCustoId { get; set; }
        public CentroCustoDto CentroCusto { get; set; }

        public long? TipoLeitoId { get; set; }
        public TipoLeitoDto TipoLeito { get; set; }
        public string MedicoSolicitante { get; set; }
        public List<LaudoMovimentoItemDto> LaudoMovimentoItensDto { get; set; }
        public string ExamesJson { get; set; }
        public long? PacienteId { get; set; }

        public int? VolumeContrasteTotal { get; set; }
        public int? VolumeContrasteVenoso { get; set; }
        public int? VolumeContrasteOral { get; set; }
        public int? VolumeContrasteRetal { get; set; }
        public bool IsIonico { get; set; }
        public bool IsBombaInsufora { get; set; }
        public bool IsContrasteVenoso { get; set; }
        public bool IsContrasteOral { get; set; }
        public bool IsContrasteRetal { get; set; }
        public string LoteContraste { get; set; }

        public string Crm { get; set; }

        public long? TurnoId { get; set; }
        public TurnoDto Turno { get; set; }

        public long? TipoAcomodacaoId { get; set; }
        public TipoAcomodacaoDto TipoAcomodacao { get; set; }

        public long? TecnicoId { get; set; }
        public TecnicoDto Tecnico { get; set; }

        public long? MedicoSolicitanteId { get; set; }
        public MedicoDto Medico { get; set; }

        public long? Ionico { get; set; }
        public long? Aplicacao { get; set; }

        public long? SolicitacaoExameItemId { get; set; }
        public SolicitacaoExameItemDto SolicitacaoExameItem { get; set; }


        public static LaudoMovimento Mapear(LaudoMovimentoDto laudoMovimentoDto)
        {
            var laudoMovimento = new LaudoMovimento();

            laudoMovimento.Id = laudoMovimentoDto.Id;
            laudoMovimento.Codigo = laudoMovimentoDto.Codigo;
            laudoMovimento.Descricao = laudoMovimentoDto.Descricao;

            laudoMovimento.AtendimentoId = laudoMovimentoDto.AtendimentoId;
            laudoMovimento.LaudoMovimentoStatusId = laudoMovimentoDto.LaudoMovimentoStatusId;
            laudoMovimento.ConvenioId = laudoMovimentoDto.ConvenioId;
            laudoMovimento.LeitoId = laudoMovimentoDto.LeitoId;
            laudoMovimento.IsContraste = laudoMovimentoDto.IsContraste;
            laudoMovimento.QtdeConstraste = laudoMovimentoDto.QtdeConstraste;
            laudoMovimento.Obs = laudoMovimentoDto.Obs;
            laudoMovimento.DataRegistro = laudoMovimentoDto.DataRegistro ?? DateTime.MinValue;
            laudoMovimento.CentroCustoId = laudoMovimentoDto.CentroCustoId;
            laudoMovimento.MedicoSolicitante = laudoMovimentoDto.MedicoSolicitante;
            laudoMovimento.VolumeContrasteTotal = laudoMovimentoDto.VolumeContrasteTotal;
            laudoMovimento.VolumeContrasteVenoso = laudoMovimentoDto.VolumeContrasteVenoso;
            laudoMovimento.VolumeContrasteOral = laudoMovimentoDto.VolumeContrasteOral;
            laudoMovimento.VolumeContrasteRetal = laudoMovimentoDto.VolumeContrasteRetal;
            laudoMovimento.IsIonico = laudoMovimentoDto.IsIonico;
            laudoMovimento.IsBombaInsufora = laudoMovimentoDto.IsBombaInsufora;
            laudoMovimento.IsContrasteVenoso = laudoMovimentoDto.IsContrasteVenoso;
            laudoMovimento.IsContrasteOral = laudoMovimentoDto.IsContrasteOral;
            laudoMovimento.IsContrasteRetal = laudoMovimentoDto.IsContrasteRetal;
            laudoMovimento.LoteContraste = laudoMovimentoDto.LoteContraste;
            laudoMovimento.Crm = laudoMovimentoDto.Crm;
            laudoMovimento.TurnoId = laudoMovimentoDto.TurnoId;
            laudoMovimento.TipoAcomodacaoId = laudoMovimentoDto.TipoAcomodacaoId;
            laudoMovimento.TecnicoId = laudoMovimentoDto.TecnicoId;
            laudoMovimento.MedicoSolicitanteId = laudoMovimentoDto.MedicoSolicitanteId;

            laudoMovimento.SolicitacaoExameItemId = laudoMovimentoDto.SolicitacaoExameItemId;

            //laudoMovimento.SolicitacaoExameItem = SolicitacaoExameItemDto.Mapear(laudoMovimentoDto.SolicitacaoExameItem);

            if (laudoMovimentoDto.SolicitacaoExameItem != null)
            {
                laudoMovimento.SolicitacaoExameItem = SolicitacaoExameItemDto.Mapear(laudoMovimentoDto.SolicitacaoExameItem);
            }

             return laudoMovimento;
        }

        public static LaudoMovimentoDto Mapear(LaudoMovimento laudoMovimento)
        {
            var laudoMovimentoDto = new LaudoMovimentoDto();

            laudoMovimentoDto.Id = laudoMovimento.Id;
            laudoMovimentoDto.Codigo = laudoMovimento.Codigo;
            laudoMovimentoDto.Descricao = laudoMovimento.Descricao;

            laudoMovimentoDto.AtendimentoId = laudoMovimento.AtendimentoId;
            laudoMovimentoDto.LaudoMovimentoStatusId = laudoMovimento.LaudoMovimentoStatusId;
            laudoMovimentoDto.ConvenioId = laudoMovimento.ConvenioId;
            laudoMovimentoDto.LeitoId = laudoMovimento.LeitoId;
            laudoMovimentoDto.IsContraste = laudoMovimento.IsContraste;
            laudoMovimentoDto.QtdeConstraste = laudoMovimento.QtdeConstraste;
            laudoMovimentoDto.Obs = laudoMovimento.Obs;
            laudoMovimentoDto.DataRegistro = laudoMovimento.DataRegistro;
            laudoMovimentoDto.CentroCustoId = laudoMovimento.CentroCustoId;
            laudoMovimentoDto.MedicoSolicitante = laudoMovimento.MedicoSolicitante;
            laudoMovimentoDto.VolumeContrasteTotal = laudoMovimento.VolumeContrasteTotal;
            laudoMovimentoDto.VolumeContrasteVenoso = laudoMovimento.VolumeContrasteVenoso;
            laudoMovimentoDto.VolumeContrasteOral = laudoMovimento.VolumeContrasteOral;
            laudoMovimentoDto.VolumeContrasteRetal = laudoMovimento.VolumeContrasteRetal;
            laudoMovimentoDto.IsIonico = laudoMovimento.IsIonico;
            laudoMovimentoDto.IsBombaInsufora = laudoMovimento.IsBombaInsufora;
            laudoMovimentoDto.IsContrasteVenoso = laudoMovimento.IsContrasteVenoso;
            laudoMovimentoDto.IsContrasteOral = laudoMovimento.IsContrasteOral;
            laudoMovimentoDto.IsContrasteRetal = laudoMovimento.IsContrasteRetal;
            laudoMovimentoDto.LoteContraste = laudoMovimento.LoteContraste;
            laudoMovimentoDto.Crm = laudoMovimento.Crm;
            laudoMovimentoDto.TurnoId = laudoMovimento.TurnoId;
            laudoMovimentoDto.TipoAcomodacaoId = laudoMovimento.TipoAcomodacaoId;
            laudoMovimentoDto.TecnicoId = laudoMovimento.TecnicoId;
            laudoMovimentoDto.MedicoSolicitanteId = laudoMovimento.MedicoSolicitanteId;

            laudoMovimento.SolicitacaoExameItemId = laudoMovimentoDto.SolicitacaoExameItemId;

            if (laudoMovimento.SolicitacaoExameItem != null)
            {
                laudoMovimentoDto.SolicitacaoExameItem = SolicitacaoExameItemDto.Mapear(laudoMovimento.SolicitacaoExameItem);
            }

            if (laudoMovimento.Atendimento != null)
            {
                laudoMovimentoDto.Atendimento = AtendimentoDto.Mapear(laudoMovimento.Atendimento);
            }

            if (laudoMovimento.LaudoMovimentoStatus != null)
            {
                laudoMovimentoDto.LaudoMovimentoStatus = LaudoMovimentoStatusDto.Mapear(laudoMovimento.LaudoMovimentoStatus);
            }

            if (laudoMovimento.Convenio != null)
            {
                laudoMovimentoDto.Convenio = ConvenioDto.Mapear(laudoMovimento.Convenio);
            }

            if (laudoMovimento.Leito != null)
            {
                laudoMovimentoDto.Leito = LeitoDto.Mapear(laudoMovimento.Leito);
            }

            if (laudoMovimento.CentroCusto != null)
            {
                laudoMovimentoDto.CentroCusto = CentroCustoDto.Mapear(laudoMovimento.CentroCusto);
            }

            if (laudoMovimento.Turno != null)
            {
                laudoMovimentoDto.Turno = TurnoDto.Mapear(laudoMovimento.Turno);
            }

            if (laudoMovimento.TipoAcomodacao != null)
            {
                laudoMovimentoDto.TipoAcomodacao = TipoAcomodacaoDto.Mapear(laudoMovimento.TipoAcomodacao);
            }


            if (laudoMovimento.Tecnico != null)
            {
                laudoMovimentoDto.Tecnico = TecnicoDto.Mapear(laudoMovimento.Tecnico);
            }

            if (laudoMovimento.Medico != null)
            {
                laudoMovimentoDto.Medico = MedicoDto.Mapear(laudoMovimento.Medico);
            }



            return laudoMovimentoDto;
        }

    }
}
