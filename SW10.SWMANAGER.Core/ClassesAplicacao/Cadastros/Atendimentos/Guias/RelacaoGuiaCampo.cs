using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias
{
    [Table("RelacaoGuiaCampo")]
    public class RelacaoGuiaCampo : CamposPadraoCRUD
    {
        public float CoordenadaX { get; set; }

        public float CoordenadaY { get; set; }

        public long GuiaId { get; set; }
        [ForeignKey("GuiaId")]
        public Guia Guia { get; set; }

        public long GuiaCampoId { get; set; }
        [ForeignKey("GuiaCampoId")]
        public GuiaCampo GuiaCampo { get; set; }
    }
}

