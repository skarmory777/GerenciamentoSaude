using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabResultadoLaudo")]
    public class ResultadoLaudo : CamposPadraoCRUD
    {
        public long? ResultadoExameId { get; set; }
        public long? ItemResultadoId { get; set; }
        public long? TabelaResultadoId { get; set; }
        public long? UnidadeId { get; set; }
        public long? UsuarioLaudoId { get; set; }

        public double Numerico { get; set; }
        public string Resultado { get; set; }
        public string Referencia { get; set; }
        [Index("Lab_Idx_DataDigitadoLaudo")]
        public DateTime? DataDigitadoLaudo { get; set; }
        public string VersaoAtual { get; set; }
        public bool IsInterface { get; set; }

        [ForeignKey("ResultadoExameId")]
        public ResultadoExame ResultadoExame { get; set; }

        [ForeignKey("ItemResultadoId")]
        public ItemResultado ItemResultado { get; set; }

        [ForeignKey("TabelaResultadoId")]
        public TabelaResultado TabelaResultado { get; set; }

        [ForeignKey("UnidadeId")]
        public LaboratorioUnidade LaboratorioUnidade { get; set; }

        public long? TipoResultadoId { get; set; }

        [ForeignKey("TipoResultadoId")]
        public TipoResultado TipoResultado { get; set; }

        public int? CasaDecimal { get; set; }

        public string Formula { get; set; }
        public int? Ordem { get; set; }
        public int? OrdemRegistro { get; set; }

        public decimal? MinimoAceitavelMasculino { get; set; }
        public decimal? MaximoAceitavelMasculino { get; set; }
        public decimal? MinimoMasculino { get; set; }
        public decimal? MaximoMasculino { get; set; }
        public decimal? NormalMasculino { get; set; }


        public decimal? MinimoAceitavelFeminino { get; set; }
        public decimal? MaximoAceitavelFeminino { get; set; }
        public decimal? MinimoFeminino { get; set; }
        public decimal? MaximoFeminino { get; set; }
        public decimal? NormalFeminino { get; set; }

        // public string TextoImpresso { get; set; }
    }
}