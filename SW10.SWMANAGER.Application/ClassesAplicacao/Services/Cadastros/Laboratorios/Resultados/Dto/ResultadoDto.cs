using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Terceirizados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;

using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto
{
    [AutoMap(typeof(Resultado))]
    public class ResultadoDto : CamposPadraoCRUDDto
    {
        public long? TecnicoId { get; set; }
        public long? FaturamentoContaId { get; set; }
        public long? ResponsavelId { get; set; }
        public long? UsuarioConferidoId { get; set; }//int] NULL,
        public long? UsuarioDigitadoId { get; set; }//int] NULL,
        public long? UsuarioEntregaId { get; set; }//int] NULL,
        public long? UsuarioCienteId { get; set; }//int] NULL,
        public long? TecnicoColetaId { get; set; }//int] NULL,
        public long? LeitoAtualId { get; set; }//int] NULL,
        public long? LocalAtualId { get; set; }//int] NULL,
        public long? RotinaId { get; set; }//int] NULL,
        public long? RequisicaoMovId { get; set; }//int] NULL,
        public long? MedicoSolicitanteId { get; set; }
        public long? AtendimentoId { get; set; }


        public TecnicoDto Tecnico { get; set; }

        public AtendimentoDto Atendimento { get; set; }

        public MedicoDto MedicoSolicitante { get; set; }

        public TecnicoDto Responsavel { get; set; }

        public User UsuarioConferido { get; set; }

        public User UsuarioDigitado { get; set; }

        public User UsuarioEntrega { get; set; }

        public User UsuarioCiente { get; set; }

        public TecnicoDto TecnicoColeta { get; set; }

        public LeitoDto LeitoAtual { get; set; }

        public UnidadeOrganizacionalDto LocalAtual { get; set; }

        public bool IsRn { get; set; }//dbo].[TBitControl] NULL,

        public bool IsEmail { get; set; }//dbo].[TBitControl] NULL,

        public bool IsEmergencia { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsUrgente { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsAvisoLab { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsAvisoMed { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsVisualiza { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsRotina { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsTransferencia { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsCiente { get; set; }//dbo].[TBitControl] NOT NULL,

        public string Numero { get; set; }
        public long Nic { get; set; }
        public DateTime? DataColeta { get; set; }
        public long? SexoRnId { get; set; }//dbo].[TSexo] NULL,	    
        public DateTime? DataDigitado { get; set; }//dbo].[TDateTime] NULL,	    
        public DateTime? DataConferido { get; set; }//dbo].[TDateTime] NULL,	    
        public DateTime? DataEnvioEmail { get; set; }//datetime] NULL,
        public DateTime? DataEntregaExame { get; set; }//datetime] NULL,	    
        public string ObsEntrega { get; set; }//varchar](100) NULL,
        public string PessoaEntrega { get; set; }//varchar](50) NULL,
        public DateTime? DataPrevEntregaExame { get; set; }//dbo].[TDateTime] NULL,
        public string Gemelar { get; set; }//varchar](5) NULL,	    
        public DateTime? DataTecnico { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataUsuarioCiente { get; set; }//dbo].[TDateTime] NULL,
        public string Peso { get; set; }//varchar](10) NULL,
        public string Altura { get; set; }//varchar](10) NULL,
        public string Remedio { get; set; }//varchar](500) NULL,

        public string ResultadosExamesList { get; set; }
        public string PacienteNome { get; set; }

        public long AmbulatorioInternacao { get; set; }


        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }

        public long? CentroCustoId { get; set; }
        public CentroCustoDto CentroCusto { get; set; }

        public long? TipoAcomodacaoId { get; set; }
        public TipoAcomodacaoDto TipoAcomodacao { get; set; }

        public long? TurnoId { get; set; }
        public TurnoDto Turno { get; set; }

        public string NomeMedicoSolicitante { get; set; }
        public string CRMSolicitante { get; set; }

        public long? ResultadoStatusId { get; set; }
        public ResultadoStatusDto ResultadoStatus { get; set; }

        public long? AutorizacaoProcedimentoId { get; set; }
        public AutorizacaoProcedimentoDto AutorizacaoProcedimento { get; set; }

        public long? LocalUtilizacaoId { get; set; }
        public UnidadeOrganizacionalDto LocalUtilizacao { get; set; }

        public long? TerceirizadoId { get; set; }
        public TerceirizadoDto Terceirizado { get; set; }

        public long? SolicitacaoExameId { get; set; }
        public SolicitacaoExameDto SolicitacaoExame { get; set; }
        public bool IsPendencia { get; set; }
        public long? PendenciaUserId { get; set; }
        public DateTime? PendenciaDateTime { get; set; }

        public string MotivoPendencia { get; set; }


        #region Mapeamento

        public static Resultado Mapear(ResultadoDto resultadoDto)
        {
            var resultado = new Resultado
            {
                Id = resultadoDto.Id,
                Codigo = resultadoDto.Codigo,
                Descricao = resultadoDto.Descricao,
                TecnicoId = resultadoDto.TecnicoId,
                FaturamentoContaId = resultadoDto.FaturamentoContaId,
                ResponsavelId = resultadoDto.ResponsavelId,
                UsuarioConferidoId = resultadoDto.UsuarioConferidoId,
                UsuarioDigitadoId = resultadoDto.UsuarioDigitadoId,
                UsuarioEntregaId = resultadoDto.UsuarioEntregaId,
                UsuarioCienteId = resultadoDto.UsuarioCienteId,
                TecnicoColetaId = resultadoDto.TecnicoColetaId,
                LeitoAtualId = resultadoDto.LeitoAtualId,
                LocalAtualId = resultadoDto.LocalAtualId,
                RotinaId = resultadoDto.RotinaId,
                RequisicaoMovId = resultadoDto.RequisicaoMovId,
                MedicoSolicitanteId = resultadoDto.MedicoSolicitanteId,
                AtendimentoId = resultadoDto.AtendimentoId,
                IsRn = resultadoDto.IsRn,
                IsEmail = resultadoDto.IsEmail,
                IsEmergencia = resultadoDto.IsEmergencia,
                IsUrgente = resultadoDto.IsUrgente,
                IsAvisoLab = resultadoDto.IsAvisoLab,
                IsAvisoMed = resultadoDto.IsAvisoMed,
                IsVisualiza = resultadoDto.IsVisualiza,
                IsRotina = resultadoDto.IsRotina,
                IsTransferencia = resultadoDto.IsTransferencia,
                IsCiente = resultadoDto.IsCiente,
                Numero = resultadoDto.Numero,
                Nic = resultadoDto.Nic,
                DataColeta = resultadoDto.DataColeta,
                SexoRnId = resultadoDto.SexoRnId,
                DataDigitado = resultadoDto.DataDigitado,
                DataConferido = resultadoDto.DataConferido,
                DataEnvioEmail = resultadoDto.DataEnvioEmail,
                DataEntregaExame = resultadoDto.DataEntregaExame,
                ObsEntrega = resultadoDto.ObsEntrega,
                PessoaEntrega = resultadoDto.PessoaEntrega,
                DataPrevEntregaExame = resultadoDto.DataPrevEntregaExame,
                Gemelar = resultadoDto.Gemelar,
                DataTecnico = resultadoDto.DataTecnico,
                DataUsuarioCiente = resultadoDto.DataUsuarioCiente,
                Peso = resultadoDto.Peso,
                Altura = resultadoDto.Altura,
                Remedio = resultadoDto.Remedio,
                NomeMedicoSolicitante = resultadoDto.NomeMedicoSolicitante,
                CRMSolicitante = resultadoDto.CRMSolicitante,
                LocalUtilizacaoId = resultadoDto.LocalUtilizacaoId,
                TerceirizadoId = resultadoDto.TerceirizadoId,
                CentroCustoId = resultadoDto.CentroCustoId,
                ResultadoStatusId = resultadoDto.ResultadoStatusId,
                SolicitacaoExameId = resultadoDto.SolicitacaoExameId,
                IsPendencia = resultadoDto.IsPendencia,
                PendenciaDateTime = resultadoDto.PendenciaDateTime,
                MotivoPendencia = resultadoDto.MotivoPendencia,
                PendenciaUserId = resultadoDto.PendenciaUserId
            };

            return resultado;

        }


        public static ResultadoDto Mapear(Resultado resultado)
        {
            var resultadoDto = new ResultadoDto
            {
                Id = resultado.Id,
                Codigo = resultado.Codigo,
                Descricao = resultado.Descricao,
                TecnicoId = resultado.TecnicoId,
                FaturamentoContaId = resultado.FaturamentoContaId,
                ResponsavelId = resultado.ResponsavelId,
                UsuarioConferidoId = resultado.UsuarioConferidoId,
                UsuarioDigitadoId = resultado.UsuarioDigitadoId,
                UsuarioEntregaId = resultado.UsuarioEntregaId,
                UsuarioCienteId = resultado.UsuarioCienteId,
                TecnicoColetaId = resultado.TecnicoColetaId,
                LeitoAtualId = resultado.LeitoAtualId,
                LocalAtualId = resultado.LocalAtualId,
                RotinaId = resultado.RotinaId,
                RequisicaoMovId = resultado.RequisicaoMovId,
                MedicoSolicitanteId = resultado.MedicoSolicitanteId,
                AtendimentoId = resultado.AtendimentoId,
                IsRn = resultado.IsRn,
                IsEmail = resultado.IsEmail,
                IsEmergencia = resultado.IsEmergencia,
                IsUrgente = resultado.IsUrgente,
                IsAvisoLab = resultado.IsAvisoLab,
                IsAvisoMed = resultado.IsAvisoMed,
                IsVisualiza = resultado.IsVisualiza,
                IsRotina = resultado.IsRotina,
                IsTransferencia = resultado.IsTransferencia,
                IsCiente = resultado.IsCiente,
                Numero = resultado.Numero,
                Nic = resultado.Nic,
                DataColeta = resultado.DataColeta,
                SexoRnId = resultado.SexoRnId,
                DataDigitado = resultado.DataDigitado,
                DataConferido = resultado.DataConferido,
                DataEnvioEmail = resultado.DataEnvioEmail,
                DataEntregaExame = resultado.DataEntregaExame,
                ObsEntrega = resultado.ObsEntrega,
                PessoaEntrega = resultado.PessoaEntrega,
                DataPrevEntregaExame = resultado.DataPrevEntregaExame,
                Gemelar = resultado.Gemelar,
                DataTecnico = resultado.DataTecnico,
                DataUsuarioCiente = resultado.DataUsuarioCiente,
                Peso = resultado.Peso,
                Altura = resultado.Altura,
                Remedio = resultado.Remedio,
                NomeMedicoSolicitante = resultado.NomeMedicoSolicitante,
                CRMSolicitante = resultado.CRMSolicitante,
                LocalUtilizacaoId = resultado.LocalUtilizacaoId,
                TerceirizadoId = resultado.TerceirizadoId,
                CentroCustoId = resultado.CentroCustoId,
                ResultadoStatusId = resultado.ResultadoStatusId,
                SolicitacaoExameId = resultado.SolicitacaoExameId,
                IsPendencia = resultado.IsPendencia,
                PendenciaDateTime = resultado.PendenciaDateTime,
                MotivoPendencia = resultado.MotivoPendencia,
                PendenciaUserId = resultado.PendenciaUserId
            };

            if (resultado.Tecnico != null)
            {
                resultadoDto.Tecnico = TecnicoDto.Mapear(resultado.Tecnico);
            }

            if (resultado.MedicoSolicitante != null)
            {
                resultadoDto.MedicoSolicitante = MedicoDto.Mapear(resultado.MedicoSolicitante);
            }

            if (resultado.Atendimento != null)
            {
                resultadoDto.Atendimento = AtendimentoDto.Mapear(resultado.Atendimento);
            }

            if (resultado.Responsavel != null)
            {
                resultadoDto.Responsavel = TecnicoDto.Mapear(resultado.Responsavel);
            }

            if (resultado.TecnicoColeta != null)
            {
                resultadoDto.TecnicoColeta = TecnicoDto.Mapear(resultado.TecnicoColeta);
            }

            if (resultado.LeitoAtual != null)
            {
                resultadoDto.LeitoAtual = LeitoDto.Mapear(resultado.LeitoAtual);
            }

            if (resultado.LocalAtual != null)
            {
                resultadoDto.LocalAtual = UnidadeOrganizacionalDto.Mapear(resultado.LocalAtual);
            }

            resultadoDto.ConvenioId = resultado.ConvenioId;

            if (resultado.Convenio != null)
            {
                resultadoDto.Convenio = ConvenioDto.Mapear(resultado.Convenio);
            }

            resultadoDto.CentroCustoId = resultado.CentroCustoId;

            if (resultado.CentroCusto != null)
            {
                resultadoDto.CentroCusto = CentroCustoDto.Mapear(resultado.CentroCusto);
            }

            resultadoDto.TipoAcomodacaoId = resultado.TipoAcomodacaoId;

            if (resultado.TipoAcomodacao != null)
            {
                resultadoDto.TipoAcomodacao = TipoAcomodacaoDto.Mapear(resultado.TipoAcomodacao);
            }

            resultadoDto.TurnoId = resultado.TurnoId;

            if (resultado.Turno != null)
            {
                resultadoDto.Turno = TurnoDto.Mapear(resultado.Turno);
            }

            if (resultado.Terceirizado != null)
            {
                resultadoDto.Terceirizado = resultado.Terceirizado.MapTo<TerceirizadoDto>();
            }

            if (resultado.LocalUtilizacao != null)
            {
                resultadoDto.LocalUtilizacao = UnidadeOrganizacionalDto.Mapear(resultado.LocalUtilizacao);
            }

            if (resultado.ResultadoStatus != null)
            {
                resultadoDto.ResultadoStatus = ResultadoStatusDto.Mapear(resultado.ResultadoStatus);
            }

            if(resultado.SolicitacaoExame != null)
            {
                resultadoDto.SolicitacaoExame = SolicitacaoExameDto.Mapear(resultado.SolicitacaoExame);
            }

            return resultadoDto;

        }

        public static ResultadoDto MapearSolicitacaoParaResultado(SolicitacaoExameDto solicitacaoExameDto)
        {
            var isToday = solicitacaoExameDto.DataSolicitacao.Date == DateTime.Today;
            
            var resultado = new ResultadoDto
            {
                DataColeta = solicitacaoExameDto.DataSolicitacao,
                AtendimentoId = solicitacaoExameDto.AtendimentoId,
                ConvenioId = solicitacaoExameDto.Atendimento?.ConvenioId,
                MedicoSolicitanteId = solicitacaoExameDto.MedicoSolicitanteId,
                AmbulatorioInternacao = solicitacaoExameDto.Atendimento.IsAmbulatorioEmergencia ? 1 : 2,
                IsUrgente = isToday && solicitacaoExameDto.Atendimento.IsAmbulatorioEmergencia,
                IsRotina = !isToday || !solicitacaoExameDto.Atendimento.IsAmbulatorioEmergencia,
                SolicitacaoExameId = solicitacaoExameDto.Id
            };
            if (solicitacaoExameDto.Atendimento.IsAmbulatorioEmergencia)
            {
                resultado.LocalUtilizacaoId = solicitacaoExameDto.Atendimento.UnidadeOrganizacionalId;
            }
            else
            {
                resultado.TipoAcomodacaoId = solicitacaoExameDto.Atendimento.TipoAcomodacaoId;
                resultado.LeitoAtualId = solicitacaoExameDto.Atendimento.LeitoId;
                resultado.LocalUtilizacaoId = solicitacaoExameDto.Atendimento.Leito?.UnidadeOrganizacionalId;
            }

            return resultado;
        }
        
        public static string OcorrenciaVoltarStatus(Resultado resultado, string statusAnterior, string statusAtual, string userName, DateTime data) => 
            $"Coleta {resultado.Nic}  status alterado de {statusAnterior} para {statusAtual} por {userName} às {data} ";

        public static List<ResultadoDto> Mapear(List<Resultado> list)
        {
            var listDto = new List<ResultadoDto>();

            foreach (var item in list)
            {
                listDto.Add(Mapear(item));
            }

            return listDto;
        }

        public static List<Resultado> Mapear(List<ResultadoDto> listDto)
        {
            var list = new List<Resultado>();

            foreach (var item in listDto)
            {
                list.Add(Mapear(item));
            }

            return list;
        }

        #endregion
    }
}
