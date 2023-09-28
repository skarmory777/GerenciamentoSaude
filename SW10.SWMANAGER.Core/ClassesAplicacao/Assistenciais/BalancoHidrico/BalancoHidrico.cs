// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalancoHidrico.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the BalancoHidrico type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using Castle.Core.Internal;
using SW10.SWMANAGER.Helper;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.BalancoHidrico
{
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The balanco hidrico.
    /// </summary>
    [Table("AteBalancoHidricos")]
    public class BalancoHidrico : CamposPadraoCRUD
    {
        /// <summary>
        /// Gets or sets the atend id.
        /// </summary>
        [ForeignKey("Atendimento")]
        public long AtendimentoId { get; set; }

        /// <summary>
        /// Gets or sets the atendimento.
        /// </summary>
        public Atendimento Atendimento { get; set; }

        [NotMapped]
        public double DiasNaAcomodacao { get; set; }

        [NotMapped]
        public string TipoAcomodacao { get; set; }

        [Index("Ate_Idx_DataBalancoHidrico")]
        /// <summary>
        /// Gets or sets the data balanco hidrico.
        /// </summary>
        public DateTime DataBalancoHidrico { get; set; }

        /// <summary>
        /// Gets or sets the balanco hidrico items.
        /// </summary>
        public ICollection<BalancoHidricoItem> BalancoHidricoItems { get; set; }

        /// <summary>
        /// Gets or sets the balanco hidrico solucoes.
        /// </summary>
        public ICollection<BalancoHidricoSolucoes> BalancoHidricoSolucoes { get; set; }

        /// <summary>
        /// Gets or sets the hora intervalo.
        /// </summary>
        public int HoraIntervalo { get; set; }
        
        public bool ConferidoManha { get; set; }
        
        public long? ConferidoManhaUserId { get; set; }
        
        public DateTime? DtConferidoManha { get; set; }
        
        
        public bool ConferidoNoite { get; set; }
        
        public long? ConferidoNoiteUserId { get; set; }
        
        public DateTime? DtConferidoNoite { get; set; }
        
        public bool ConferidoTotal { get; set; }
        
        public long? ConferidoTotalUserId { get; set; }
        
        public DateTime? DtConferidoTotal { get; set; }
        
        public bool DesConferidoManha { get; set; }
        
        public long? DesConferidoManhaUserId { get; set; }
        
        public DateTime? DtDesConferidoManha { get; set; }
        
        public bool DesConferidoNoite { get; set; }
        
        public long? DesConferidoNoiteUserId { get; set; }
        
        public DateTime? DtDesConferidoNoite { get; set; }
        
        public bool DesConferidoTotal { get; set; }
        
        public long? DesConferidoTotalUserId { get; set; }
        
        public DateTime? DtDesConferidoTotal { get; set; }
        
        public bool? Evacuacoes { get; set; }
        
        public string Aspecto { get; set; }
    }

    /// <summary>
    /// The balanco hidrico item.
    /// </summary>
    [Table("AteBalancoHidricoItens")]
    public class BalancoHidricoItem : CamposPadraoCRUD
    {
        /// <summary>
        /// Gets or sets the balanco hidrico id.
        /// </summary>
        [ForeignKey("BalancoHidrico")]
        public long BalancoHidricoId { get; set; }

        /// <summary>
        /// Gets or sets the balanco hidrico.
        /// </summary>
        public BalancoHidrico BalancoHidrico { get; set; }

        /// <summary>
        /// Gets or sets the hora.
        /// </summary>
        public TimeSpan Hora { get; set; }

        /// <summary>
        /// Gets or sets the sinais vitais.
        /// </summary>
        [ForeignKey("SinaisVitais")]
        public long SinaisVitaisId { get; set; }

        /// <summary>
        /// Gets or sets the sinais vitais.
        /// </summary>
        public BalancoHidricoSinaisVitais SinaisVitais { get; set; }

        /// <summary>
        /// Gets or sets the endovenosos.
        /// </summary>
        public ICollection<BalancoHidricoEndovenoso> Endovenosos { get; set; }

        /// <summary>
        /// Gets or sets the sangue derivados.
        /// </summary>
        public string SangueDerivados { get; set; }

        /// <summary>
        /// Gets or sets the Enteral.
        /// </summary>
        public string Enteral { get; set; }

        /// <summary>
        /// Gets or sets the ingest vo sne.
        /// </summary>
        public string IngestVoSne { get; set; }

        /// <summary>
        /// Gets or sets the diurese.
        /// </summary>
        public string Diurese { get; set; }

        /// <summary>
        /// Gets or sets the hd.
        /// </summary>
        public string Hd { get; set; }

        /// <summary>
        /// Gets or sets the dreno.
        /// </summary>
        public string Dreno { get; set; }

        /// <summary>
        /// Gets or sets the dreno.
        /// </summary>
        public string Dreno2 { get; set; }
        
        public string IrrigacaodeEntrada  { get; set; }
        
        public string IrrigacaodeSaida { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether total parcial.
        /// </summary>
        public bool TotalParcial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether total geral.
        /// </summary>
        public bool TotalGeral { get; set; }
        
        public bool TotalTransporte { get; set; }

        /// <summary>
        /// Gets or sets the responsavel assinatura.
        /// </summary>
        public long ResponsavelAssinatura { get; set; }

        public static BalancoHidricoItem Mapear(BalancoHidricoItem itemPrincipal, BalancoHidricoItem itemAcopiar)
        {
            
            // itemPrincipal.Id = itemAcopiar.Id;
            // itemPrincipal.Codigo = itemAcopiar.Codigo;
            // itemPrincipal.Descricao = itemAcopiar.Descricao;
            //
            // itemPrincipal.IsDeleted = itemAcopiar.IsDeleted;
            // itemPrincipal.DeleterUserId = itemAcopiar.DeleterUserId;
            // itemPrincipal.DeletionTime = itemAcopiar.DeletionTime;
            // itemPrincipal.CreatorUserId = itemAcopiar.CreatorUserId;
            // itemPrincipal.CreationTime = itemAcopiar.CreationTime;
            // itemPrincipal.IsSistema = itemAcopiar.IsSistema;
            // itemPrincipal.LastModificationTime = itemAcopiar.LastModificationTime;
            // itemPrincipal.LastModifierUserId = itemAcopiar.LastModifierUserId;
            // itemPrincipal.ImportaId = itemAcopiar.ImportaId;
            
            itemPrincipal.Diurese = itemAcopiar.Diurese;
            itemPrincipal.Dreno = itemAcopiar.Dreno;
            itemPrincipal.Dreno2 = itemAcopiar.Dreno2;
            itemPrincipal.Enteral = itemAcopiar.Enteral;
            itemPrincipal.Hd = itemAcopiar.Hd;
            itemPrincipal.Hora = itemAcopiar.Hora;
            itemPrincipal.ResponsavelAssinatura = itemAcopiar.ResponsavelAssinatura;
            itemPrincipal.SangueDerivados = itemPrincipal.SangueDerivados;
            itemPrincipal.TotalGeral = itemAcopiar.TotalGeral;
            itemPrincipal.TotalParcial = itemAcopiar.TotalParcial;
            itemPrincipal.IngestVoSne = itemAcopiar.IngestVoSne;

            if (itemPrincipal.SinaisVitais == null)
            {
                itemPrincipal.SinaisVitais = new BalancoHidricoSinaisVitais();
            }

            if (itemAcopiar.SinaisVitais != null)
            {
                itemPrincipal.SinaisVitais.Hemoglucoteste = itemAcopiar.SinaisVitais.Hemoglucoteste;
                itemPrincipal.SinaisVitais.Ins = itemAcopiar.SinaisVitais.Ins;
                itemPrincipal.SinaisVitais.Pulso = itemAcopiar.SinaisVitais.Pulso;
                itemPrincipal.SinaisVitais.Respiracao = itemAcopiar.SinaisVitais.Respiracao;
                itemPrincipal.SinaisVitais.Spo2 = itemAcopiar.SinaisVitais.Spo2;
                itemPrincipal.SinaisVitais.Temperatura = itemAcopiar.SinaisVitais.Temperatura;
                itemPrincipal.SinaisVitais.PressaoDiastolica = itemAcopiar.SinaisVitais.PressaoDiastolica;
                itemPrincipal.SinaisVitais.PressaoSistolica = itemAcopiar.SinaisVitais.PressaoSistolica;
                itemPrincipal.SinaisVitais.EscalaDeDor = itemAcopiar.SinaisVitais.EscalaDeDor;
                itemPrincipal.SinaisVitais.PressaoVenosaCentral = itemAcopiar.SinaisVitais.PressaoVenosaCentral;
            }

            if (itemAcopiar.Endovenosos.IsNullOrEmpty())
            {
                return itemPrincipal;
            }
            
            if (itemPrincipal.Endovenosos.IsNullOrEmpty())
            {
                itemPrincipal.Endovenosos = new List<BalancoHidricoEndovenoso>();
            }
            
            foreach (var aCopiarEndovenoso in itemAcopiar.Endovenosos)
            {
                var endovenosoPrincipal = itemPrincipal.Endovenosos.FirstOrDefault(x => x.IndiceSolucao == aCopiarEndovenoso.IndiceSolucao);
                if (endovenosoPrincipal == null)
                {
                    itemPrincipal.Endovenosos.Add(aCopiarEndovenoso);
                }
                else
                {
                    endovenosoPrincipal.Valor = aCopiarEndovenoso.Valor;
                }
            }
            return itemPrincipal;
        }
    }

    /// <summary>
    /// The balanco hidrico endovenoso.
    /// </summary>
    [Table("AteBalancoHidricoEndovenosos")]
    public class BalancoHidricoEndovenoso : CamposPadraoCRUD
    {
        /// <summary>
        /// Gets or sets the balanco hidrico item id.
        /// </summary>
        [ForeignKey("BalancoHidricoItem")]
        public long BalancoHidricoItemId { get; set; }

        /// <summary>
        /// Gets or sets the balanco hidrico item.
        /// </summary>
        public BalancoHidricoItem BalancoHidricoItem { get; set; }

        /// <summary>
        /// Gets or sets the indice solucao.
        /// </summary>
        public int IndiceSolucao { get; set; }

        /// <summary>
        /// Gets or sets the valor.
        /// </summary>
        public string Valor { get; set; }
    }

    [Table("AteBalancoHidricoSinaisVitais")]
    public class BalancoHidricoSinaisVitais : CamposPadraoCRUD
    {
        /// <summary>
        /// Gets or sets the temperatura.
        /// </summary>
        public string Temperatura { get; set; }

        /// <summary>
        /// Gets or sets the pulso.
        /// </summary>
        public string Pulso { get; set; }

        /// <summary>
        /// Gets or sets the respiracao.
        /// </summary>
        public string Respiracao { get; set; }

        public string Spo2 { get; set; }

        /// <summary>
        /// Gets or sets the pressao sistolica.
        /// </summary>
        public string PressaoSistolica { get; set; }

        /// <summary>
        /// Gets or sets the pressao diastolica.
        /// </summary>
        public string PressaoDiastolica { get; set; }

        /// <summary>
        /// Gets or sets the pressao venosa central.
        /// </summary>
        public string PressaoVenosaCentral { get; set; }

        /// <summary>
        /// Gets or sets the escala de dor.
        /// </summary>
        public string EscalaDeDor { get; set; }

        /// <summary>
        /// Gets or sets the hemoglucoteste.
        /// </summary>
        public string Hemoglucoteste { get; set; }

        public string Ins { get; set; }
        
        public string PressaoIntracraniana { get; set; }
        
       
    }

    /// <summary>
    /// The balanco hidrico solucoes.
    /// </summary>
    [Table("AteBalancoHidricoSolucoes")]
    public class BalancoHidricoSolucoes : CamposPadraoCRUD
    {
        /// <summary>
        /// Gets or sets the balanco hidrico id.
        /// </summary>
        [ForeignKey("BalancoHidrico")]
        public long BalancoHidricoId { get; set; }

        /// <summary>
        /// Gets or sets the balanco hidrico.
        /// </summary>
        public BalancoHidrico BalancoHidrico { get; set; }

        /// <summary>
        /// Gets or sets the indice solucao.
        /// </summary>
        public int IndiceSolucao { get; set; }

        /// <summary>
        /// Gets or sets the valor.
        /// </summary>
        public string Valor { get; set; }
    }

    /// <summary>
    /// The balanco hidrico 24 hrs view model.
    /// </summary>
    public class BalancoHidrico24HrsViewModel
    {
        /// <summary>
        /// Gets or sets the iv.
        /// </summary>
        public string Iv { get; set; }

        public string SeD { get; set; }

        public string IeVO { get; set; }

        /// <summary>
        /// Gets or sets the tp intro.
        /// </summary>
        public string TpIntro { get; set; }

        /// <summary>
        /// Gets or sets the diur.
        /// </summary>
        public string Diur { get; set; }

        /// <summary>
        /// Gets or sets the dreno.
        /// </summary>
        public string Dreno { get; set; }

        /// <summary>
        /// Gets or sets the dreno.
        /// </summary>
        public string Dreno2 { get; set; }

        /// <summary>
        /// Enteral
        /// </summary>
        public string Enteral { get;set; }

        /// <summary>
        /// HD
        /// </summary>
        public string Hd { get; set; }

        /// <summary>
        /// Gets or sets the tg.
        /// </summary>
        public string TG { get; set; }

        /// <summary>
        /// Gets or sets the balanco cumulativo.
        /// </summary>
        public string BalancoCumulativo { get; set; }

        /// <summary>
        /// Gets or sets the tp eli.
        /// </summary>
        public string TpEli { get; set; }

        /// <summary>
        /// Gets or sets the atualizado.
        /// </summary>
        public string Atualizado { get; set; }

        /// <summary>
        /// Gets or sets the balanco atual.
        /// </summary>
        public string BalancoAtual { get; set; }
    }


    public class LimitViewModel
    {
        public double MinDangerValue { get; set; }
        public double MaxDangerValue { get; set; }
        
        public double MinLowerWarningValue { get; set; }
        public double MaxLowerWarningValue { get; set; }
        
        public double MinHighWarningValue { get; set; }
        public double MaxHighWarningValue { get; set; }

        public bool GetDanger(double? value)
        {
            if (!value.HasValue)
            {
                return false;
            }
            return value.Value.LessOrEqual(MinDangerValue) || value.Value.GreaterOrEqual(MaxDangerValue);
        }
        
        public bool GetWarning(double? value)
        {
            if (!value.HasValue)
            {
                return false;
            }

            var min = value.Value.GreaterOrEqual(MinLowerWarningValue) && value.Value.LessOrEqual(MaxLowerWarningValue);
            var max = value.Value.GreaterOrEqual(MinHighWarningValue) && value.Value.LessOrEqual(MaxHighWarningValue);
            return min || max;
        }
    }

    public class SinalVitalViewModel: IComparable<SinalVitalViewModel>
    {
        public SinalVitalViewModel()
        {
            
        }
        public SinalVitalViewModel(TimeSpan hora, string valor)
        {
            Hora = hora;
            Valor = valor;
        }
        public TimeSpan Hora { get; set; }
        
        public string Valor { get; set; }

        public double? Number
        {
            get
            {
                var culture = new CultureInfo("pt-br");
                return FormatterHelper.StringToDoubleNullable(Valor, culture);
            }
        }

        public int CompareTo(SinalVitalViewModel obj)
        {
            return Nullable.Compare(Number,obj.Number);
        }
    }
}
