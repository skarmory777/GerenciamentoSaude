using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CEP;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.VersoesTiss;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.CodigoCredenciado;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios
{
    [Table("SisConvenio")]
    public class Convenio : CamposPadraoCRUD
    {
        public Convenio()
        {
            //if(this.SisPessoa==null)
            //{
            //    this.SisPessoa = new SisPessoa();
            //} 
        }

        public string NomeFantasia { get { return (this.SisPessoa != null) ? this.SisPessoa.NomeFantasia : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.NomeFantasia = value; } }


        public string RazaoSocial { get { return (this.SisPessoa != null) ? this.SisPessoa.RazaoSocial : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.RazaoSocial = value; } }


        public string Cnpj { get { return (this.SisPessoa != null) ? this.SisPessoa.Cnpj : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.Cnpj = value; } }

        public string InscricaoEstadual { get { return (this.SisPessoa != null) ? this.SisPessoa.InscricaoEstadual : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.InscricaoEstadual = value; } }

        public string InscricaoMunicipal { get { return (this.SisPessoa != null) ? this.SisPessoa.InscricaoMunicipal : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.InscricaoMunicipal = value; } }



        #region propridades de Pessoa

        [StringLength(9)]
        public string Cep { get; set; }

        //ACERTAR REFERENCIA
        public long? TipoLogradouroId { get; set; }

        [ForeignKey("TipoLogradouroId")]
        public TipoLogradouro TipoLogradouro { get; set; }

        [StringLength(80)]
        public string Logradouro { get; set; }

        [StringLength(30)]
        public string Complemento { get; set; }

        [StringLength(20)]
        public string Numero { get; set; }

        [StringLength(40)]
        public string Bairro { get; set; }

        [ForeignKey("CidadeId")]
        public Cidade Cidade { get; set; }
        public long? CidadeId { get; set; }

        [ForeignKey("EstadoId")]
        public Estado Estado { get; set; }
        public long? EstadoId { get; set; }

        [ForeignKey("PaisId")]
        public Pais Pais { get; set; }
        public long? PaisId { get; set; }

        [StringLength(20)]
        public string Telefone1 { get { return (this.SisPessoa != null) ? this.SisPessoa.Telefone1 : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.Telefone1 = value; } }

        public int? TipoTelefone1 { get; set; }

        public int? DddTelefone1 { get; set; }

        [StringLength(20)]
        public string Telefone2 { get { return (this.SisPessoa != null) ? this.SisPessoa.Telefone2 : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.Telefone2 = value; } }

        public int? TipoTelefone2 { get; set; }

        public int? DddTelefone2 { get; set; }

        [StringLength(20)]
        public string Telefone3 { get { return (this.SisPessoa != null) ? this.SisPessoa.Telefone3 : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.Telefone3 = value; } }

        public int? TipoTelefone3 { get; set; }

        public int? DddTelefone3 { get; set; }

        [StringLength(20)]
        public string Telefone4 { get { return (this.SisPessoa != null) ? this.SisPessoa.Telefone4 : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.Telefone4 = value; } }

        public int? TipoTelefone4 { get; set; }

        public int? DddTelefone4 { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get { return (this.SisPessoa != null) ? this.SisPessoa.Email : string.Empty; } set { if (this.SisPessoa != null) this.SisPessoa.Email = value; } }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Email4 { get; set; }


        #endregion



        //CONFIRMAR NOME DO CAMPO.
        public string Nome { get; set; }

        public bool IsAtivo { get; set; }

        public byte[] Logotipo { get; set; }

        public string LogotipoMimeType { get; set; }

        public bool IsFilotranpica { get; set; }

        [StringLength(255)]
        public string LogradouroCobranca { get; set; }

        [ForeignKey("CepCobranca"), Column("SisCepCobrancaId")]
        public long? CepCobrancaId { get; set; }

        public Cep CepCobranca { get; set; }

        [ForeignKey("TipoLogradouroCobranca"), Column("SisTipoLogradouroCobrancaId")]
        public long? TipoLogradouroCobrancaId { get; set; }

        public TipoLogradouro TipoLogradouroCobranca { get; set; }

        public string NumeroCobranca { get; set; }

        public string ComplementoCobranca { get; set; }

        public string BairroCobranca { get; set; }

        [ForeignKey("CidadeCobranca"), Column("SisCidadeCobrancaId")]
        public long? CidadeCobrancaId { get; set; }

        public Cidade CidadeCobranca { get; set; }

        [ForeignKey("EstadoCobranca"), Column("SisEstadoCobrancaId")]
        public long? EstadoCobrancaId { get; set; }

        public Estado EstadoCobranca { get; set; }

        public string Cargo { get; set; }

        public DateTime DataInicioContrato { get; set; }

        public int Vigencia { get; set; }

        public DateTime DataProximaRenovacaoContratual { get; set; }

        public DateTime DataInicialContrato { get; set; }

        public DateTime DataUltimaRenovacaoContrato { get; set; }

        public string RegistroANS { get; set; }

        // Novo modelo SisPessoa
        // Novo modelo SisPessoa
        [ForeignKey("SisPessoa"), Column("SisPessoaId")]
        public long? SisPessoaId { get; set; }
        public SisPessoa SisPessoa { get; set; }

        public long? VersaoTissId { get; set; }

        [ForeignKey("VersaoTissId")]
        public VersaoTiss VersaoTiss { get; set; }

        //Campos de configuração do TISS
        public bool IsPreencheCodigoCredenciadoCodigoOperadora { get; set; }
        public bool IsImprimeTratamento { get; set; }
        public bool IsImprimeObsConta { get; set; }
        public bool IsAgrupaGuiaXml { get; set; }
        public bool Is09e10 { get; set; }
        public bool IsFatorMultiplicador { get; set; }
        public bool IsEquipeMedicaBranco { get; set; }
        public bool IsObrigaEspecialidade { get; set; }
        public bool Is41a45BrancoPJ { get; set; }
        public bool IsSomarFilmeTaxa { get; set; }
        public bool IsImprimeObsContaGuiaConsulta { get; set; }
        public bool IsImportaAgudaCronica { get; set; }
        public bool Is38e39GuiaConsulta { get; set; }
        public bool Is86e89GuiaSPSADT { get; set; }
        public bool IsFilmeGuiaOutrasDespesas { get; set; }
        public bool Is22GuiaSPSADT { get; set; }
        public int XmlUltimosDigitosMatricula { get; set; }
        public int XmlPrimeirosDigitosMatricula { get; set; }

        public bool IsEletivo { get; set; }
        public bool IsUrgencia { get; set; }

        public List<IdentificacaoPrestadorNaOperadora> IdentificacoesPrestadoresNaOperadora { get; set; }

        //public IList<ConvenioURLServico> UrlServicos { get; set; }

        public string VerificaElegibilidadeHomologacao { get; set; }
        public string ComunicacaoBeneficiarioHomologacao { get; set; }
        public string CancelaGuiaHomologacao { get; set; }
        public string SolicitacaoProcedimentoHomologacao { get; set; }
        public string SolicitastatusAutorizacaoHomologacao { get; set; }
        public string LoteAnexoHomologacao { get; set; }
        public string LoteGuiasHomologacao { get; set; }
        public string SolicitaStatusProtocoloHomologacao { get; set; }
        public string SolicitacaoDemonstrativoRetornoHomologacao { get; set; }
        public string RecursoGlosaHomologacao { get; set; }

        public string VerificaElegibilidade { get; set; }
        public string ComunicacaoBeneficiario { get; set; }
        public string CancelaGuia { get; set; }
        public string SolicitacaoProcedimento { get; set; }
        public string SolicitastatusAutorizacao { get; set; }
        public string LoteAnexo { get; set; }
        public string LoteGuias { get; set; }
        public string SolicitaStatusProtocolo { get; set; }
        public string SolicitacaoDemonstrativoRetorno { get; set; }
        public string RecursoGlosa { get; set; }

        public string WebService { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Homologacao { get; set; }
        public string Certificado { get; set; }
        public string CaCerfts { get; set; }
        public string SenhaCertificado { get; set; }
        public string SenhaCacerts { get; set; }


        public bool IsCaixa { get; set; }

        public long? FormaAutorizacaoId { get; set; }

        [ForeignKey("FormaAutorizacaoId")]
        public FormaAutorizacao FormaAutorizacao { get; set; }

        public string DadosContato { get; set; }

        public bool IsPreencheGuiaAutomaticamente { get; set; }

        public long? EmpresaPadraoEmergenciaId { get; set; }

        [ForeignKey("EmpresaPadraoEmergenciaId")]
        public Empresa EmpresaPadraoEmergencia { get; set; }

        public long? MedicoPadraoEmergenciaId { get; set; }

        [ForeignKey("MedicoPadraoEmergenciaId")]
        public Medico MedicoPadraoEmergencia { get; set; }

        public long? EspecialidadePadraoEmergenciaId { get; set; }

        [ForeignKey("EspecialidadePadraoEmergenciaId")]
        public Especialidade EspecialidadePadraoEmergencia { get; set; }

        public long? EmpresaPadraoInternacaoId { get; set; }

        [ForeignKey("EmpresaPadraoInternacaoId")]
        public Empresa EmpresaPadraoInternacao { get; set; }

        public long? MedicoPadraoInternacaoId { get; set; }

        [ForeignKey("MedicoPadraoInternacaoId")]
        public Medico MedicoPadraoInternacao { get; set; }

        public long? EspecialidadePadraoInternacaoId { get; set; }

        [ForeignKey("EspecialidadePadraoInternacaoId")]
        public Especialidade EspecialidadePadraoInternacao { get; set; }

        public List<IntervaloGuiasConvenio> IntervalosGuiasConvenios { get; set; }

        public List<CodigoCredenciado> CodigosCredenciado { get; set; }

        public List<FaturamentoGrupoConvenio> FatGrupoConvenio { get; set; }
        
        public bool IsParticular { get; set; }

        public bool HabilitaAuditoriaInterna { get; set; }

        public bool HabilitaAuditoriaExterna { get; set; }

        public bool HabilitaEntrega { get; set; }

        public bool IsAgrupaResumoConta { get; set; }
        public bool IsAgrupaItensResumoConta { get; set; }

        public long? ConfiguracaoResumoContaInternacaoId { get; set; }

        [ForeignKey("ConfiguracaoResumoContaInternacaoId")]
        public ConvenioConfiguracaoResumoConta ConfiguracaoResumoContaInternacao { get; set; }

        public long? ConfiguracaoResumoContaEmergenciaId { get; set; }

        [ForeignKey("ConfiguracaoResumoContaEmergenciaId")]
        public ConvenioConfiguracaoResumoConta ConfiguracaoResumoContaEmergencia { get; set; }

    }

    public class ConvenioConfiguracaoResumoConta: CamposPadraoCRUD
    {
        public bool IsAgrupaItens { get; set; }
        public bool IsAgrupaUnidadeOrganizacional { get; set; }
        public bool IsImprimeCID { get; set; }
        public bool IsImprimeCPFMedico { get; set; }
        public bool IsImprimeObservacaoItens { get; set; }

        public bool IsImprimeTratamento { get; set; }
        public bool IsImprimeTratamentos { get; set; }

        public bool IsImprimeDataHoraImpressao { get; set; }

        public bool IsExibeDescontoDoCaixa { get; set; }

    }

    //public class ConvenioConfiguracaoResumoContaAgrupaUnidadeOrganizacional: CamposPadraoCRUD
    //{
    //    public long ConvenioConfiguracaoResumoContaId { get; set; }
    //    public ConvenioConfiguracaoResumoConta ResumoConta { get; set; }

    //    public long UnidadeOrganizacionalId { get; set; }

    //    public UnidadeOrganizacional UnidadeOrganizacional { get; set; }
    //}
}
