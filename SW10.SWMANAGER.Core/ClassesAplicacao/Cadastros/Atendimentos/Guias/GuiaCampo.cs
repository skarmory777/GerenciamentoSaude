using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias
{
    [Table("GuiaCampo")]
    public class GuiaCampo : CamposPadraoCRUD
    {
        public float CoordenadaX { get; set; }

        public float CoordenadaY { get; set; }

        public bool IsConjunto { get; set; }

        public bool IsSubItem { get; set; }

        public long? ConjuntoId { get; set; }

        public int MaximoElementos { get; set; }// aqui deve ser nullable provavelmente
    }
}