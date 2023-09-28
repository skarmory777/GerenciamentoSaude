using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.TiposAcompanhantes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.ServicosMedicosPrestados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.TiposAtendimento;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCID;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Nacionalidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Origens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos
{
    [Table("AteAtendimento")]
    public class Atendimento : CamposPadraoCRUD
    {
        public string GuiaNumero { get; set; }

        public string Matricula { get; set; }

        public string Responsavel { get; set; }

        public string RgResponsavel { get; set; }

        public string CpfResponsavel { get; set; }

        public string NumeroGuia { get; set; }

        public int? QtdSessoes { get; set; }

        [Index("Ate_Idx_DataRetorno")]
        public DateTime? DataRetorno { get; set; }

        [Index("Ate_Idx_DataRevisao")]
        public DateTime? DataRevisao { get; set; }

        [Index("Ate_Idx_DataPreatendimento")]
        public DateTime? DataPreatendimento { get; set; }

        [Index("Ate_Idx_DataPrevistaAtendimento")]
        public DateTime? DataPrevistaAtendimento { get; set; }

        [Index("Ate_Idx_DataRegistro")]
        public DateTime DataRegistro { get; set; }

        // ALTAS
        [Index("Ate_Idx_DataAlta")]
        public DateTime? DataAlta { get; set; }

        [Index("Ate_Idx_DataPrevistaAlta")]
        public DateTime? DataPrevistaAlta { get; set; }

        [ForeignKey("AltaGrupoCID"), Column("AteGrupoCIDId")]
        public long? AltaGrupoCIDId { get; set; }
        public GrupoCID AltaGrupoCID { get; set; }

        [ForeignKey("MotivoAlta"), Column("AteMotivoAltaId")]
        public long? MotivoAltaId { get; set; }
        public MotivoAlta MotivoAlta { get; set; }

        [ForeignKey("TipoAcompanhante"), Column("AteTipoAcompanhanteId")]
        public long? TipoAcompanhanteId { get; set; }
        public TipoAcompanhante TipoAcompanhante { get; set; }

        [Index("Ate_Idx_DataAltaMedica")]
        public DateTime? DataAltaMedica { get; set; }

        [ForeignKey("Leito"), Column("AteLeitoId")]
        public long? LeitoId { get; set; }
        public Leito Leito { get; set; }

        public string NumeroObito { get; set; }
        // FIM - ALTAS

        public DateTime? ValidadeCarteira { get; set; }

        public DateTime? ValidadeSenha { get; set; }

        public DateTime? DataAutorizacao { get; set; }

        public long? DiasAutorizacao { get; set; }

        public DateTime? DataUltimoPagamento { get; set; }

        public string Senha { get; set; }

        public string Parentesco { get; set; }

        public string Titular { get; set; }


        public string Observacao { get; set; }

        [Index("Ate_Idx_IsAmbulatorioEmergencia")]
        public bool IsAmbulatorioEmergencia { get; set; }

        [Index("Ate_Idx_IsInternacao")]
        public bool IsInternacao { get; set; }

        [Index("Ate_Idx_IsHomeCare")]
        public bool IsHomeCare { get; set; }

        [Index("Ate_Idx_IsPreatendimento")]
        public bool IsPreatendimento { get; set; }

        public bool IsControlaTev { get; set; }

        [ForeignKey("Paciente"), Column("SisPacienteId")]
        public long? PacienteId { get; set; }

        [ForeignKey("Origem"), Column("SisOrigemId")]
        public long? OrigemId { get; set; }

        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long? MedicoId { get; set; }

        [ForeignKey("Especialidade"), Column("SisEspecialidadeId")]
        public long? EspecialidadeId { get; set; }

        [ForeignKey("Empresa"), Column("SisEmpresaId")]
        public long? EmpresaId { get; set; }

        [ForeignKey("Convenio"), Column("SisConveniolId")]
        public long? ConvenioId { get; set; }

        [ForeignKey("Plano"), Column("SisPlanoId")]
        public long? PlanoId { get; set; }

        [ForeignKey("TipoAcomodacao"), Column("SisTipoAcomodacaoId")]
        public long? TipoAcomodacaoId { get; set; }

        [ForeignKey("AtendimentoStatus"), Column("AteAtendimentoStatusId")]
        public long? AtendimentoStatusId { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }

        [ForeignKey("AtendimentoTipo"), Column("SisAtendimentoTipoId")]
        public long? AtendimentoTipoId { get; set; }

        // Modelo antigo
        [ForeignKey("Guia"), Column("SisGuiaId")]
        public long? GuiaId { get; set; }
        // Novo modelo FatGuia
        [ForeignKey("FatGuia"), Column("FatGuiaId")]
        public long? FatGuiaId { get; set; }
        public FaturamentoGuia FatGuia { get; set; }

        [ForeignKey("ServicoMedicoPrestado"), Column("SisServicoMedicoPrestadoId")]
        public long? ServicoMedicoPrestadoId { get; set; }

        [ForeignKey("Nacionalidade"), Column("SisNacionalidadeResponsavelId")]
        public long? NacionalidadeResponsavelId { get; set; }

        public Nacionalidade Nacionalidade { get; set; }

        public Paciente Paciente { get; set; }

        public TipoAcomodacao TipoAcomodacao { get; set; }

        public Origem Origem { get; set; }

        public Medico Medico { get; set; }

        public Especialidade Especialidade { get; set; }

        public Empresa Empresa { get; set; }

        public Convenio Convenio { get; set; }

        public Plano Plano { get; set; }

        public TipoAtendimento AtendimentoTipo { get; set; }

        public ServicoMedicoPrestado ServicoMedicoPrestado { get; set; }

        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        public AtendimentoStatus AtendimentoStatus { get; set; }
        
        [ForeignKey("FaturamentoAtendimentoStatus"),Column("FatAtendimentoStatusId")]
        public long? FaturamentoAtendimentoStatusId { get; set; }
        
        public FaturamentoAtendimentoStatus FaturamentoAtendimentoStatus { get; set; }

        // modelo antigo
        public Guia Guia { get; set; }

        // Reflexion
        public object this[string propertyName]
        {
            get
            {
                string[] props = propertyName.Split('.');
                if (props.Length == 2)
                {
                    switch (props[0])
                    {
                        case "Paciente":
                            return this.Paciente[props[1]];
                        case "Medico":
                            return this.Medico[props[1]];
                        case "Plano":
                            return this.Plano[props[1]];
                    }
                }

                return this.GetType().GetProperty(propertyName).GetValue(this, null);
            }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        [ForeignKey("AtendimentoMotivoCancelamentoId")]
        public AtendimentoMotivoCancelamento AtendimentoMotivoCancelamento { get; set; }

        public long? AtendimentoMotivoCancelamentoId { get; set; }

        public string CNS { get; set; }

        public long? CaraterAtendimentoId { get; set; }

        [ForeignKey("CaraterAtendimentoId")]
        public TabelaDominio CaraterAtendimento { get; set; }

        public long? IndicacaoAcidenteId { get; set; }

        [ForeignKey("IndicacaoAcidenteId")]
        public TabelaDominio IndicacaoAcidente { get; set; }

        public string CodDependente { get; set; }

        public long? ClassificacaoAtendimentoId { get; set; }

        [ForeignKey("ClassificacaoAtendimentoId")]
        public ClassificacaoAtendimento ClassificacaoAtendimento { get; set; }

        public long? ProtocoloAtendimentoId { get; set; }

        [ForeignKey("ProtocoloAtendimentoId")]
        public ProtocoloAtendimento ProtocoloAtendimento { get; set; }

        public bool IsPendenteExames { get; set; }
        public bool IsPendenteMedicacao { get; set; }
        public bool IsPendenteProcedimento { get; set; }
        public bool IsAtendidoInternado { get; set; }
        public bool IsAtendidoAlta { get; set; }
        public bool IsAtendidoAguardandoInternacao { get; set; }

        public int? StatusAguardando { get; set; }
        public int? StatusAtendido { get; set; }

        public long? AtendimentoOrigemId { get; set; }

        [ForeignKey("AtendimentoOrigemId")]
        public Atendimento AtendimentoOrigem { get; set; }

        public int OrigemTitular { get; set; }
        public DateTime? DataTomadaDecisao { get; set; }

        public long? UsuarioTomadaDecisao { get; set; }
    }
}
