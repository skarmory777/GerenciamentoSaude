using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaPrecoItens;
using SW10.SWMANAGER.ClassesAplicacao.SisMoedas;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas
{
    [Table("FatContaItem")]
    public class FaturamentoContaItem : CamposPadraoCRUD
    {
        [StringLength(10)]
        public override string Codigo { get; set; }

        [StringLength(100)]
        public override string Descricao { get; set; }

        [ForeignKey("FaturamentoItem"), Column("FatItemId")]
        public long? FaturamentoItemId { get; set; }
        public FaturamentoItem FaturamentoItem { get; set; }

        [ForeignKey("FaturamentoConta"), Column("FatContaId")]
        public long? FaturamentoContaId { get; set; }
        public FaturamentoConta FaturamentoConta { get; set; }

        [Index("Fat_Idx_Data")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset? Data { get; set; }

        public float Qtde { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("UnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [ForeignKey("Terceirizado"), Column("TerceirizadoId")]
        public long? TerceirizadoId { get; set; }
        public Terceirizado Terceirizado { get; set; }

        [ForeignKey("CentroCusto"), Column("CentroCustoId")]
        public long? CentroCustoId { get; set; }
        public CentroCusto CentroCusto { get; set; }

        [ForeignKey("Turno"), Column("TurnoId")]
        public long? TurnoId { get; set; }
        public Turno Turno { get; set; }

        // [ForeignKey("TipoLeito"), Column("TipoLeitoId")]
        //public long? TipoLeitoId { get; set; }
        // public TipoLeito TipoLeito { get; set; }

        [ForeignKey("TipoAcomodacao"), Column("TipoAcomodacaoId")]
        public long? TipoAcomodacaoId { get; set; }
        public TipoAcomodacao TipoAcomodacao { get; set; }

        public float ValorTemp { get; set; }

        [ForeignKey("Medico"), Column("MedicoId")]
        public long? MedicoId { get; set; }
        public Medico Medico { get; set; }

        public bool IsMedCredenciado { get; set; }

        public bool IsGlosaMedico { get; set; }

        [ForeignKey("MedicoEspecialidade"), Column("MedicoEspecialidadeId")]
        public long? MedicoEspecialidadeId { get; set; }
        public MedicoEspecialidade MedicoEspecialidade { get; set; }

        //[ForeignKey("FaturamentoPacote"), Column("FaturamentoPacoteId")]
        //public long? FaturamentoPacoteId { get; set; }
        //public FaturamentoPacote FaturamentoPacote { get; set; }

        [ForeignKey("FaturamentoContaKit"), Column("FaturamentoContaKitId")]
        public long? FaturamentoContaKitId { get; set; }
        public FaturamentoContaKit FaturamentoContaKit { get; set; }

        public bool IsCirurgia { get; set; }

        public float ValorAprovado { get; set; }
        public float ValorTaxas { get; set; }
        public bool IsValorItemManual { get; set; }
        public float ValorItem { get; set; }
        public string HMCH { get; set; }
        public float ValorFilme { get; set; }
        public float ValorFilmeAprovado { get; set; }
        public float ValorCOCH { get; set; }
        public float ValorCOCHAprovado { get; set; }
        public float Percentual { get; set; }
        public bool IsInstrCredenciado { get; set; }
        public float ValorTotalRecuperado { get; set; }
        public float ValorTotalRecebido { get; set; }
        public float MetragemFilme { get; set; }
        public float MetragemFilmeAprovada { get; set; }
        public float COCH { get; set; }
        public float COCHAprovado { get; set; }

        [ForeignKey("Status"), Column("FatContaStatusId")]
        public long? StatusId { get; set; }
        public FaturamentoContaStatus Status { get; set; }

        public bool IsRecuperaMedico { get; set; }

        [ForeignKey("Auxiliar1"), Column("Auxiliar1Id")]
        public long? Auxiliar1Id { get; set; }
        public Medico Auxiliar1 { get; set; }

        public bool IsAux1Credenciado { get; set; }
        public bool IsRecebeAuxiliar1 { get; set; }
        public bool IsGlosaAuxiliar1 { get; set; }
        public bool IsRecuperaAuxiliar1 { get; set; }

        [ForeignKey("Auxiliar1Especialidade"), Column("Auxiliar1EspecialidadeId")]
        public long? Auxiliar1EspecialidadeId { get; set; }
        public MedicoEspecialidade Auxiliar1Especialidade { get; set; }

        [ForeignKey("Auxiliar2"), Column("Auxiliar2Id")]
        public long? Auxiliar2Id { get; set; }
        public Medico Auxiliar2 { get; set; }

        public bool IsAux2Credenciado { get; set; }
        public bool IsRecebeAuxiliar2 { get; set; }
        public bool IsGlosaAuxiliar2 { get; set; }
        public bool IsRecuperaAuxiliar2 { get; set; }

        [ForeignKey("Auxiliar2Especialidade"), Column("Auxiliar2EspecialidadeId")]
        public long? Auxiliar2EspecialidadeId { get; set; }
        public MedicoEspecialidade Auxiliar2Especialidade { get; set; }

        [ForeignKey("Auxiliar3"), Column("Auxiliar3Id")]
        public long? Auxiliar3Id { get; set; }
        public Medico Auxiliar3 { get; set; }

        public bool IsAux3Credenciado { get; set; }
        public bool IsRecebeAuxiliar3 { get; set; }
        public bool IsGlosaAuxiliar3 { get; set; }
        public bool IsRecuperaAuxiliar3 { get; set; }

        [ForeignKey("Auxiliar3Especialidade"), Column("Auxiliar3EspecialidadeId")]
        public long? Auxiliar3EspecialidadeId { get; set; }
        public MedicoEspecialidade Auxiliar3Especialidade { get; set; }

        [ForeignKey("Instrumentador"), Column("InstrumentadorId")]
        public long? InstrumentadorId { get; set; }
        public Medico Instrumentador { get; set; }

        public bool IsRecebeInstrumentador { get; set; }
        public bool IsGlosaInstrumentador { get; set; }
        public bool IsRecuperaInstrumentador { get; set; }

        [ForeignKey("InstrumentadorEspecialidade"), Column("InstrumentadorEspecialidadeId")]
        public long? InstrumentadorEspecialidadeId { get; set; }
        public MedicoEspecialidade InstrumentadorEspecialidade { get; set; }

        [ForeignKey("Anestesista"), Column("AnestesistaId")]
        public long? AnestesistaId { get; set; }
        public Medico Anestesista { get; set; }

        [ForeignKey("AnestesistaEspecialidade"), Column("AnestesistaEspecialidadeId")]
        public long? AnestesistaEspecialidadeId { get; set; }
        public MedicoEspecialidade AnestesistaEspecialidade { get; set; }
        public bool IsAnestCredenciado { get; set; }

        public string Observacao { get; set; }

        public int QtdeRecuperada { get; set; }
        public int QtdeAprovada { get; set; }
        public int QtdeRecebida { get; set; }

        //[ForeignKey("EntregaLote"), Column("EntregaLoteId")]
        //public long? EntregaLoteId { get; set; }
        //public EntregaLote EntregaLote { get; set; }

        public float ValorMoedaAprovado { get; set; }

        [ForeignKey("SisMoeda"), Column("SisMoedaId")]
        public long? SisMoedaId { get; set; }
        public SisMoeda SisMoeda { get; set; }

        //[ForeignKey("Autorizacao"), Column("AutorizacaoId")]
        //public long? AutorizacaoId { get; set; }
        //public Autorizacao Autorizacao { get; set; }
        [Index("Fat_Idx_DataAutorizacao")]
        [DataType(DataType.DateTime)]
        public DateTime? DataAutorizacao { get; set; }

        public string SenhaAutorizacao { get; set; }

        public string NomeAutorizacao { get; set; }

        public string ObsAutorizacao { get; set; }

        //[ForeignKey("RequisicaoMovItem"), Column("RequisicaoMovItemId")]
        //public long? RequisicaoMovItemId { get; set; }
        //public RequisicaoMovItem RequisicaoMovItem { get; set; }

        [ForeignKey("Preco"), Column("FatPrecoId")]
        public long? PrecoId { get; set; }
        public FaturamentoTabelaPrecoItem Preco { get; set; }

        [Index("Fat_Idx_HoraIncio")]
        [DataType(DataType.DateTime)]
        public DateTime? HoraIncio { get; set; }
        [Index("Fat_Idx_HoraFim")]
        public DateTime? HoraFim { get; set; }

        public string ViaAcesso { get; set; }

        public string Tecnica { get; set; }

        public string ClinicaId { get; set; }

        [ForeignKey("Fornecedor"), Column("FornecedorId")]
        public long? FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }

        public string NumeroNF { get; set; }

        public bool IsImportaEstoque { get; set; }

        public long? FaturamentoConfigConvenioId { get; set; }

        [ForeignKey("FaturamentoConfigConvenioId")]
        public FaturamentoConfigConvenio FaturamentoConfigConvenio { get; set; }



        [ForeignKey("FaturamentoItemCobrado"), Column("FatItemCobradoId")]
        public long? FaturamentoItemCobradoId { get; set; }
        public FaturamentoItem FaturamentoItemCobrado { get; set; }

        [ForeignKey("FaturamentoPacote"), Column("FatPacoteId")]
        public long? FaturamentoPacoteId { get; set; }


        public FaturamentoPacote FaturamentoPacote { get; set; }
        
        
        public string ResumoDetalhamentoJSON { get; set; }
        
        [NotMapped]
        public ResumoDetalhamento ResumoDetalhamento
        {
            get => ResumoDetalhamentoJSON == null ? null : JsonConvert.DeserializeObject<ResumoDetalhamento>(ResumoDetalhamentoJSON);
            set => ResumoDetalhamentoJSON = JsonConvert.SerializeObject(value);
        }




        //[ForeignKey("GlosaEstMotivoRecup"), Column("GlosaEstMotivoRecupId")]
        //public long? GlosaEstMotivoRecupId { get; set; }
        //public GlosaEstMotivoRecup GlosaEstMotivoRecup { get; set; }
    }

}


