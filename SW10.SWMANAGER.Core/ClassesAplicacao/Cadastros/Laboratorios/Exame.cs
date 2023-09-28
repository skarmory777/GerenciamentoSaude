using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabExame")]
    public class Exame : CamposPadraoCRUD
    {

        public bool IsExameSimples { get; set; }

        public bool IsPeso { get; set; }

        public bool IsTesta100 { get; set; }

        public bool IsAltura { get; set; }

        public bool IsCor { get; set; }

        public bool IsMestruacao { get; set; }

        public bool IsNacionalidade { get; set; }

        public bool IsNaturalidade { get; set; }

        public bool IsImpReferencia { get; set; }

        public bool IsCultura { get; set; }

        public bool IsPendente { get; set; }

        public bool IsRepete { get; set; }

        public bool IsLibera { get; set; }

        public string Mneumonico { get; set; }
        public int? OrdemImp { get; set; }
        public int? Prazo { get; set; }
        public byte[] Interpretacao { get; set; }
        public byte[] Extra1 { get; set; }
        public byte[] Extra2 { get; set; }
        public string Referencia { get; set; }
        public int? QtdFatura { get; set; }
        public string MapaExame { get; set; }
        public int? OrdemResul { get; set; }
        public int? OrdemResumo { get; set; }
        public int? OrdemMapaResultado { get; set; }


        //   public long? FaturamentoItemId { get; set; }
        public long? EquipamentoId { get; set; }
        public long? ExameIncluiId { get; set; }
        public long? SetorId { get; set; }
        public long? MaterialId { get; set; }
        public long? MetodoId { get; set; }
        public long? UnidadeId { get; set; }
        public long? FormataId { get; set; }
        public long? MapaId { get; set; }

        //[ForeignKey("FaturamentoItemId")]
        //public FaturamentoItem FaturamentoItem { get; set; }

        [ForeignKey("EquipamentoId")]
        public Equipamento Equipamento { get; set; }

        [ForeignKey("ExameIncluiId")]
        public Exame ExameInclui { get; set; }

        [ForeignKey("SetorId")]
        public Setor Setor { get; set; }

        [ForeignKey("MaterialId")]
        public Material Material { get; set; }

        [ForeignKey("MetodoId")]
        public Metodo Metodo { get; set; }

        [ForeignKey("UnidadeId")]
        public LaboratorioUnidade Unidade { get; set; }

        [ForeignKey("FormataId")]
        public Formata Formata { get; set; }

        [ForeignKey("MapaId")]
        public Mapa Mapa { get; set; }

    }
}