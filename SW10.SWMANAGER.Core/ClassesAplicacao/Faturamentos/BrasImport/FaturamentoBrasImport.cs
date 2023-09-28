using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasImports
{
    [Table("FatBrasImport")]
    public class FaturamentoBrasImport : CamposPadraoCRUD
    {
        public string CodigoLaboratorio { get; set; }

        public string Laboratorio { get; set; }

        public string CodigoProduto { get; set; }

        public string Produto { get; set; }

        public string CodigoApresentacao { get; set; }

        public string Apresentacao { get; set; }

        public string PrecoUnitario { get; set; }

        public string PrecoTotal { get; set; }

        public string NumeroUnidades { get; set; }

        public string Tipo { get; set; }

        public string Versao { get; set; }

        public string Extra { get; set; }

        public string IsAtualizado { get; set; }

        public string CodigoBarra { get; set; }

        public string CodigoBrasTiss { get; set; }

        public string CodigoBrasTuss { get; set; }

        public string CodigoHierarquico { get; set; }
    }

}


