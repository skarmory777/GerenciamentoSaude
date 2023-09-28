using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabItemResultado")]
    public class ItemResultado : CamposPadraoCRUD
    {

        public int? CasaDecimal { get; set; }
        //public double ErroMinimo { get; set; }
        //public double ErroMaximo { get; set; }
        //public double AlteradoMinimo { get; set; }
        //public double AlteradoMaximo { get; set; }
        //public double AceitavelMinimo { get; set; }
        //public double AceitavelMaximo { get; set; }
        public string Referencia { get; set; }
        public string Formula { get; set; }

        // public double Normal { get; set; }
        public int? TamFixo { get; set; }
        public string ObsAnormal { get; set; }
        //public double ErroMinimoFeminino { get; set; }
        //public double AlteradoMinimoFeminino { get; set; }
        //public double NormalFeminino { get; set; }
        //public double AceitavelMaximoFeminino { get; set; }
        //public double ErroMaximoFeminino { get; set; }
        public string Interface { get; set; }
        public string InterfaceEnvio { get; set; }
        //public string Equipamento { get; set; }
        public double DivideInter { get; set; }

        public bool IsAntibiotico { get; set; }

        public bool IsBacteria { get; set; }

        public bool IsInteiro { get; set; }

        public bool IsObrigatorio { get; set; }

        public bool IsMultiValor { get; set; }

        public bool IsSoma100 { get; set; }

        public bool ParteInteira { get; set; }

        public bool IsInterface { get; set; }

        public bool IsTamFixo { get; set; }

        public long? TipoResultadoId { get; set; }
        public long? LaboratorioUnidadeId { get; set; }
        public long? TabelaId { get; set; }

        [ForeignKey("Equipamento"), Column("LabEquipamentoId")]
        public long? EquipamentoId { get; set; }

        [ForeignKey("TabelaId")]
        public Tabela Tabela { get; set; }

        [ForeignKey("LaboratorioUnidadeId")]
        public LaboratorioUnidade LaboratorioUnidade { get; set; }

        [ForeignKey("TipoResultadoId")]
        public TipoResultado TipoResultado { get; set; }

        public Equipamento Equipamento { get; set; }

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

    }
}