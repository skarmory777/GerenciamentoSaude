using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Home
{
    [AutoMap(typeof(AtendimentoDto))]
    public class AssistenciaisViewModel : AtendimentoDto
    {
        public SelectList Empresas { get; set; }
        public SelectList UnidadesOrganizacionais { get; set; }
        public string Filtro { get; set; }
        public virtual ICollection<AtendimentoDto> Atendimentos { get; set; }
        public bool IsEditMode { get { return Id > 0; } }

        public List<UnidadeOrganizacionalDto> UnidadeOrganizacionais { get; internal set; }

        public List<AtendimentoStatusDto> ListaAtendimentoStatus { get; internal set; }

        public bool HabilitarAcoes { get; set; } = true;

        public AssistenciaisViewModel(AtendimentoDto atendimento)
        {
            Mapeamento(atendimento);
        }
        
        public AssistenciaisViewModel(AtendimentoDto atendimento, HeaderAtendimentoPacienteNavBarOptions options)
        {
            Mapeamento(atendimento);
            if (options == null)
            {
                options = new HeaderAtendimentoPacienteNavBarOptions();
            }
            Options = options;
        }
        
        public void Mapeamento(AtendimentoDto atendimento)
        {
            this.Id = atendimento.Id;
            this.Codigo = atendimento.Codigo;
            this.GuiaNumero = atendimento.GuiaNumero;
            this.Matricula = atendimento.Matricula;
            this.Responsavel = atendimento.Responsavel;
            this.RgResponsavel = atendimento.RgResponsavel;
            this.CpfResponsavel = atendimento.CpfResponsavel;
            this.NumeroGuia = atendimento.NumeroGuia;
            this.QtdSessoes = atendimento.QtdSessoes;
            this.Senha = atendimento.Senha;
            this.Parentesco = atendimento.Parentesco;
            this.Observacao = atendimento.Observacao;
            this.PacienteId = atendimento.PacienteId;
            this.OrigemId = atendimento.OrigemId;
            this.MedicoId = atendimento.MedicoId;
            this.EspecialidadeId = atendimento.EspecialidadeId;
            this.EmpresaId = atendimento.EmpresaId;
            this.ConvenioId = atendimento.ConvenioId;
            this.PlanoId = atendimento.PlanoId;
            this.AtendimentoStatusId = atendimento.AtendimentoStatusId;
            this.UnidadeOrganizacionalId = atendimento.UnidadeOrganizacionalId;
            this.AtendimentoTipoId = atendimento.AtendimentoTipoId;
            this.GuiaId = atendimento.GuiaId;
            this.TipoAcompanhanteId = atendimento.TipoAcompanhanteId;
            this.FatGuiaId = atendimento.FatGuiaId;
            this.LeitoId = atendimento.LeitoId;
            this.MotivoAltaId = atendimento.MotivoAltaId;
            this.NacionalidadeResponsavelId = atendimento.NacionalidadeResponsavelId;
            this.IsAmbulatorioEmergencia = atendimento.IsAmbulatorioEmergencia;
            this.IsInternacao = atendimento.IsInternacao;
            this.IsHomeCare = atendimento.IsHomeCare;
            this.IsPreatendimento = atendimento.IsPreatendimento;
            this.ServicoMedicoPrestadoId = atendimento.ServicoMedicoPrestadoId;
            this.AltaGrupoCIDId = atendimento.AltaGrupoCIDId;
            this.DataRetorno = atendimento.DataRetorno;
            this.DataRevisao = atendimento.DataRevisao;
            this.DataPreatendimento = atendimento.DataPreatendimento;
            this.DataPrevistaAtendimento = atendimento.DataPrevistaAtendimento;
            this.DataRegistro = atendimento.DataRegistro;
            this.DataAlta = atendimento.DataAlta;
            this.DataAltaMedica = atendimento.DataAltaMedica;
            this.ValidadeCarteira = atendimento.ValidadeCarteira;
            this.ValidadeSenha = atendimento.ValidadeSenha;
            this.DataAutorizacao = atendimento.DataAutorizacao;
            this.DiasAutorizacao = atendimento.DiasAutorizacao;
            this.DataPrevistaAlta = atendimento.DataPrevistaAlta;
            this.TipoAcomodacaoId = atendimento.TipoAcomodacaoId;
            this.AtendimentoTipoId = atendimento.AtendimentoTipoId;
            this.CNS = atendimento.CNS;
            this.CaraterAtendimentoId = atendimento.CaraterAtendimentoId;
            this.IndicacaoAcidenteId = atendimento.IndicacaoAcidenteId;
            this.Titular = atendimento.Titular;
            this.DataUltimoPagamento = atendimento.DataUltimoPagamento;
            this.CodDependente = atendimento.CodDependente;
            this.Paciente = atendimento.Paciente;
            this.Empresa = atendimento.Empresa;
            this.Medico = atendimento.Medico;
            this.Leito = atendimento.Leito;
            this.Convenio = atendimento.Convenio;
            this.FatGuia = atendimento.FatGuia;
            this.TipoAcompanhante = atendimento.TipoAcompanhante;
            this.AtendimentoTipo = atendimento.AtendimentoTipo;
            this.UnidadeOrganizacional = atendimento.UnidadeOrganizacional;
            this.TipoAcomodacao = atendimento.TipoAcomodacao;
            this.Plano = atendimento.Plano;
            this.Origem = atendimento.Origem;
            this.Especialidade = atendimento.Especialidade;
            this.CaraterAtendimento = atendimento.CaraterAtendimento;
            this.IndicacaoAcidente = atendimento.IndicacaoAcidente;
            this.AltaGrupoCID = atendimento.AltaGrupoCID;
            this.AtendimentoStatus = atendimento.AtendimentoStatus;
            this.MotivoAlta = atendimento.MotivoAlta;
            this.Nacionalidade = atendimento.Nacionalidade;
            this.ServicoMedicoPrestado = atendimento.ServicoMedicoPrestado;
            this.ClassificacaoAtendimentoId = atendimento.ClassificacaoAtendimentoId;
            this.ClassificacaoAtendimento = atendimento.ClassificacaoAtendimento;
            this.ProtocoloAtendimentoId = atendimento.ProtocoloAtendimentoId;
            this.ProtocoloAtendimento = atendimento.ProtocoloAtendimento;
            this.IsPendenteExames = atendimento.IsPendenteExames;
            this.IsPendenteMedicacao = atendimento.IsPendenteMedicacao;
            this.IsPendenteProcedimento = atendimento.IsPendenteProcedimento;
            this.IsAtendidoInternado = atendimento.IsAtendidoInternado;
            this.IsAtendidoAlta = atendimento.IsAtendidoAlta;
            this.IsAtendidoAguardandoInternacao = atendimento.IsAtendidoAguardandoInternacao;
            this.StatusAguardando = atendimento.StatusAguardando;
            this.StatusAtendido = atendimento.StatusAtendido;
            this.AtendimentoOrigemId = atendimento.AtendimentoOrigemId;

        }
        
        public HeaderAtendimentoPacienteNavBarOptions Options { get; set; } =
            new HeaderAtendimentoPacienteNavBarOptions();
    }

    public class HeaderAtendimentoPacienteNavBarOptions
    {
        public HeaderAtendimentoPacienteNavBarOptions()
        {
            
        }
        
        public HeaderAtendimentoPacienteNavBarOptions(bool compact = false)
        {
            Compact = compact;
            ShowActions = false;
            ShowImage = false;
        }

        public HeaderAtendimentoPacienteNavBarOptions(bool compact = false, bool showImage =true, bool showActions = true)
        {
            Compact = compact;
            ShowImage = showImage;
            ShowActions = showActions;
        }
        
        public bool Compact { get; set; }

        public bool ShowImage { get; set; } = true;

        public bool ShowActions { get; set; } = true;
    }
}